using UnityEngine;
using UnityEditor;
using System.Xml.Schema;
using System.IO;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine.Assertions;

namespace Xsd2So
{

    public class DataRepresentation
    {
        public XmlTypeMapping TypeMapping { get; set; }
        public CodeTypeDeclaration CodeType { get; set; }

        public bool IsArray { get; internal set; }
    }

    public class DataTypeRepresentation : DataRepresentation
    {
        public XmlSchemaType XsdType { get; set; }
        public bool IsComplexXsdType { get { return XsdType is XmlSchemaComplexType; } }
    }

    public class DataElementRepresentation : DataRepresentation
    {
        public XmlSchemaElement XsdType { get; set; }
    }

    class ModifiableRepresentationDuplicateComparer : IEqualityComparer<DataRepresentation>
    {
        public bool Equals(DataRepresentation x, DataRepresentation y)
        {
            if (x.TypeMapping.ElementName == y.TypeMapping.ElementName)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(DataRepresentation obj)
        {
            return obj.TypeMapping.ElementName.GetHashCode();
        }
    }

    public class XSDConverter
    {
        [MenuItem("XSD/Convert")]
        public static void Convert()
		{
			#region Rework this section to be more dynamic

			// Load XSD as text file
			var path = Application.dataPath + "/Example/XSD/test.xsd";
			var content = File.ReadAllText(path);

			var context = new GenerationContext("Example.Generated.Editor", "Example.Generated", "BalancingData", "Root");

			#endregion

			// Parse Xsd to schema code representation
			context.XsdConfig.ProcessXsd(content);

			// Generate type mapping for all Xsd types.
			XmlSchemaImporter schemaImporter = new XmlSchemaImporter(context.XsdConfig.XsdSchemas);
			var typeList = new List<DataTypeRepresentation>(context.XsdConfig.XsdSchema.SchemaTypes.Values.Count);
			foreach (XmlSchemaType schemaType in context.XsdConfig.XsdSchema.SchemaTypes.Values)
			{
				var type = schemaImporter.ImportSchemaType(schemaType.QualifiedName);
				var typeRep = new DataTypeRepresentation();
				typeRep.XsdType = schemaType;
				typeRep.TypeMapping = type;

				typeList.Add(typeRep);
			}

			// Generate type mapping for all Xsd elements.
			var elementList = new List<DataElementRepresentation>(context.XsdConfig.XsdSchema.Elements.Values.Count);
			foreach (XmlSchemaElement schemaElement in context.XsdConfig.XsdSchema.Elements.Values)
			{
				var type = schemaImporter.ImportTypeMapping(schemaElement.QualifiedName);
				var typeRep = new DataElementRepresentation();
				typeRep.XsdType = schemaElement;
				typeRep.TypeMapping = type;

				elementList.Add(typeRep);
			}

			// Export all xsd type mappings to the CodeDOM
			CodeNamespace codeNamespace = context.XmlCode;
			XmlCodeExporter codeExporter = new XmlCodeExporter(codeNamespace);
			var modifiableTypes = new List<DataRepresentation>();
			foreach (var rep in typeList)
			{
				codeExporter.ExportTypeMapping(rep.TypeMapping);
				AssociateXsdTypeWithCodeType(rep, codeNamespace);
				if (rep.CodeType != null)
				{
					modifiableTypes.Add(rep);
				}
			}

			foreach (var rep in elementList)
			{
				codeExporter.ExportTypeMapping(rep.TypeMapping);
				AssociateXsdTypeWithCodeType(rep, codeNamespace);
				if (rep.CodeType != null)
				{
					modifiableTypes.Add(rep);
				}
			}

			context.XsdCodeMapping = modifiableTypes.Distinct(new ModifiableRepresentationDuplicateComparer());

			Assert.AreEqual(codeNamespace.Types.Count, context.XsdCodeMapping.Count());

			// Modify types
			RunCodeModifiers(codeNamespace, context);

			// Check for invalid characters in identifiers
			//CodeGenerator.ValidateIdentifiers(codeNamespace); // not implemented in Unity's Mono, has to be handwritten

			// output the C# code
			CSharpCodeProvider codeProvider = new CSharpCodeProvider();
			var codeGenOptions = new CodeGeneratorOptions();

			StringBuilder xmlCode;
			using (StringWriter writer = new StringWriter())
			{
				codeProvider.GenerateCodeFromNamespace(context.XmlCode, writer, codeGenOptions);
				xmlCode = writer.GetStringBuilder();
				Debug.Log(xmlCode.ToString());
			}

			StringBuilder soCode;
			using (StringWriter writer = new StringWriter())
			{
				codeProvider.GenerateCodeFromNamespace(context.ScriptableObjectCode, writer, codeGenOptions);
				soCode = writer.GetStringBuilder();
				Debug.Log(soCode.ToString());
			}

			// TODO Hardcoded, make it dynamic
			SaveCodeToFile(xmlCode, StringHelper.PathCombine("Example", "Generated", "Editor", "XmlData.cs"));
			SaveCodeToFile(soCode, StringHelper.PathCombine("Example", "Generated", "XmlDataScriptableObject.cs"));

			// Refresh Unity to compile everything
			AssetDatabase.Refresh();
		}

		private static void SaveCodeToFile(StringBuilder xmlCode, string savePath)
		{
			var filePath = Path.Combine(Application.dataPath, savePath);
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			File.WriteAllText(filePath, xmlCode.ToString());
		}

		private static void RunCodeModifiers(CodeNamespace codeNamespace, GenerationContext context)
		{
			// TODO Hardcoded, make it dynamic
			var createSo = new ScriptableObjectGenerator("SO");
            createSo.Execute(context);
        }

        private static void AssociateXsdTypeWithCodeType(DataRepresentation rep, CodeNamespace codeNamespace)
        {
            var repXsd = rep.TypeMapping;
            string xsdTypeName = repXsd.TypeName;
            if (xsdTypeName.EndsWith("[]"))
            {
                rep.IsArray = true;
                rep.CodeType = null;
                return;
            }

            foreach (CodeTypeDeclaration type in codeNamespace.Types)
            {
                if (type.Name == xsdTypeName)
                {
                    rep.CodeType = type;
                    return;
                }
            }
        }
    }
}