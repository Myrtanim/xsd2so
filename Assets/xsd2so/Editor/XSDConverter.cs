using UnityEngine;
using System.Xml.Schema;
using System.IO;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace Xsd2So
{
    public class Xsd2So
	{
		public static void Generate(ConverterConfig config, string xsdContent)
		{
			var context = new GenerationContext(config);
			
			ProcessXsd(xsdContent, context);

			// Check for invalid characters in identifiers
			//CodeGenerator.ValidateIdentifiers(codeNamespace); // not implemented in Unity's Mono, has to be handwritten

			// Generate SO types
			var createSo = new ScriptableObjectGenerator(context.Config.SoSuffix);
			createSo.Execute(context);

			// generate the C# source code
			CSharpCodeProvider codeProvider = new CSharpCodeProvider();
			var codeGenOptions = new CodeGeneratorOptions();
			var xmlCode = GenerateCodeFileContent(context.XmlCode, codeProvider, codeGenOptions);
			var soCode = GenerateCodeFileContent(context.ScriptableObjectCode, codeProvider, codeGenOptions);
			
			SaveCodeToFile(xmlCode, config.SavePathXsdCode);
			SaveCodeToFile(soCode, config.SavePathSoCode);

			// Refresh Unity to compile everything
			//AssetDatabase.Refresh();
		}

		private static void ProcessXsd(string content, GenerationContext context)
		{
			ParseXsd(content, context);
			CompileXsd(context);
			var typeMappings = GenerateTypeMapping(context);
			var modifiableTypes = GenerateXsdCode(context, typeMappings);
			// get only distinct code mappings, because Xsd Elements and Xsd Types can potantially be the same
			context.XsdCodeMapping = modifiableTypes.Distinct(new ModifiableRepresentationDuplicateComparer());
		}

		private static List<DataRepresentation> GenerateXsdCode(GenerationContext context, List<DataRepresentation> typeMappings)
		{
			// Export all xsd type mappings to the CodeDOM
			var modifiableTypes = new List<DataRepresentation>();
			var codeExporter = new XmlCodeExporter(context.XmlCode);
			foreach (var rep in typeMappings)
			{
				codeExporter.ExportTypeMapping(rep.TypeMapping);
				AssociateXsdTypeWithCodeType(rep, context.XmlCode);
				if (rep.CodeType != null)
				{
					modifiableTypes.Add(rep);
				}
			}

			return modifiableTypes;
		}

		private static List<DataRepresentation> GenerateTypeMapping(GenerationContext context)
		{
			// Generate type mapping for all Xsd types.
			XmlSchemaImporter schemaImporter = new XmlSchemaImporter(context.XsdSchemas);
			var typeMappingList = new List<DataRepresentation>(context.XsdSchema.SchemaTypes.Values.Count + context.XsdSchema.Elements.Values.Count);
			foreach (XmlSchemaType schemaType in context.XsdSchema.SchemaTypes.Values)
			{
				var type = schemaImporter.ImportSchemaType(schemaType.QualifiedName);
				var typeRep = new DataTypeRepresentation();
				typeRep.XsdType = schemaType;
				typeRep.TypeMapping = type;

				typeMappingList.Add(typeRep);
			}

			// Generate type mapping for all Xsd elements.
			foreach (XmlSchemaElement schemaElement in context.XsdSchema.Elements.Values)
			{
				var type = schemaImporter.ImportTypeMapping(schemaElement.QualifiedName);
				var typeRep = new DataElementRepresentation();
				typeRep.XsdType = schemaElement;
				typeRep.TypeMapping = type;

				typeMappingList.Add(typeRep);
			}

			return typeMappingList;
		}

		private static void CompileXsd(GenerationContext context)
		{
			context.XsdSchemas = new XmlSchemas();
			context.XsdSchemas.Add(context.XsdSchema);
			context.XsdSchemas.Compile(null, true);
		}

		private static void ParseXsd(string content, GenerationContext context)
		{
			using (var stream = new StringReader(content))
			{
				context.XsdSchema = XmlSchema.Read(stream, null);
			}
		}

		private static void AssociateXsdTypeWithCodeType(DataRepresentation rep, CodeNamespace codeNamespace)
		{
			var repXsd = rep.TypeMapping;
			string xsdTypeName = repXsd.TypeName;
			if (xsdTypeName.EndsWith("[]", false, CultureInfo.InvariantCulture))
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

		private static StringBuilder GenerateCodeFileContent(CodeNamespace codeDom, CSharpCodeProvider codeProvider, CodeGeneratorOptions codeGenOptions)
		{
			StringBuilder xmlCode;
			using (StringWriter writer = new StringWriter())
			{
				codeProvider.GenerateCodeFromNamespace(codeDom, writer, codeGenOptions);
				xmlCode = writer.GetStringBuilder();
			}

			return xmlCode;
		}

		private static void SaveCodeToFile(StringBuilder xmlCode, string savePath)
		{
			var filePath = Path.Combine(Application.dataPath, savePath);
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			File.WriteAllText(filePath, xmlCode.ToString());
		}
    }
}