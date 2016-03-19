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
using System;
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

    class ModifiableTypeDuplicateComparer : IEqualityComparer<DataRepresentation>
    {
        public bool Equals(DataRepresentation x, DataRepresentation y)
        {
            if( x.CodeType == y.CodeType)
            {
                Debug.Log("ModifiableTypeDuplicateComparer: DUPLICATE");
                return true;
            }
            return false;
        }

        public int GetHashCode(DataRepresentation obj)
        {
            return obj.GetHashCode();
        }
    }

    public class XSDConverter
    {
        /*
            Need mapping XmlSchemaType to generated CodeDomType
            Because CodeDomType contains getters/setters for attributes and also their
                isSet state as attributes, if its an non-required XML attribute (most cases in GGS).

            But XmlSchemaType can be XmlSchemaComplexType or XmlSchemaSimpleType
                XmlSchemaSimpleType is converted to Enums
                XmlSchemaComplexType is the "real" type definition and contains attributes

            One can get the attributes list from XmlSchemaComplexType and from this get the real
            attributes from the CodeDomeType and so differentiate it from the isSet attributes.

            The isSet attribute is named "[xml attribute name]SpecifiedField", maybe this is the
            cheap solution to differentiate it from normal attributes.

            Only the non-isSet attributes from the CodeDomType have the real names, the private
            fields have some numbers appended.
        */

        [MenuItem("XSD/Convert")]
        public static void Convert()
        {
            // Load XSD as text file
            var path = Application.dataPath + "/XSD/shared.xsd";
            var content = File.ReadAllText(path);

            // Parse Xsd to schema code representation
            XmlSchema xsd;
            using (var stream = new StringReader(content))
            {
                xsd = XmlSchema.Read(stream, null);
            }

            // Compile Xsd
            XmlSchemas xsds = new XmlSchemas();
            xsds.Add(xsd);
            xsds.Compile(null, true);

            // Generate type mapping for all Xsd types.
            XmlSchemaImporter schemaImporter = new XmlSchemaImporter(xsds);
            var typeList = new List<DataTypeRepresentation>(xsd.SchemaTypes.Values.Count);
            foreach (XmlSchemaType schemaType in xsd.SchemaTypes.Values)
            {
                var type = schemaImporter.ImportSchemaType(schemaType.QualifiedName);
                var typeRep = new DataTypeRepresentation();
                typeRep.XsdType = schemaType;
                typeRep.TypeMapping = type;

                typeList.Add(typeRep);
            }

            // Generate type mapping for all Xsd elements.
            var elementList = new List<DataElementRepresentation>(xsd.Elements.Values.Count);
            foreach (XmlSchemaElement schemaElement in xsd.Elements.Values)
            {
                var type = schemaImporter.ImportTypeMapping(schemaElement.QualifiedName);
                var typeRep = new DataElementRepresentation();
                typeRep.XsdType = schemaElement;
                typeRep.TypeMapping = type;

                elementList.Add(typeRep);
            }

            // Export all xsd type mappings to the CodeDOM
            CodeNamespace codeNamespace = new CodeNamespace("Generated");
            XmlCodeExporter codeExporter = new XmlCodeExporter(codeNamespace);
            foreach (var rep in typeList)
            {
                codeExporter.ExportTypeMapping(rep.TypeMapping);
                AssociateXsdTypeWithCodeType(rep, codeNamespace);
                if (rep.CodeType == null)
                    Debug.LogError("Can't find code type for xsd type " + rep.TypeMapping.TypeName + " (element name '" + rep.TypeMapping.ElementName + "')");
            }

            foreach (var rep in elementList)
            {
                codeExporter.ExportTypeMapping(rep.TypeMapping);
                AssociateXsdTypeWithCodeType(rep, codeNamespace);
                if (rep.CodeType == null)
                    Debug.LogError("Can't find code type for xsd type " + rep.TypeMapping.TypeName + "(element name '" + rep.TypeMapping.ElementName + "')");
            }

            // Create list of modifiable types, since some xsd types/elements might not have a code type representation (e.g. because they are arrays)
            var modifiableTypes = new List<DataRepresentation>();
            modifiableTypes.AddRange(typeList.FindAll(rep => rep.CodeType != null).Cast<DataRepresentation>());
            modifiableTypes.AddRange(elementList.FindAll(rep => rep.CodeType != null).Cast<DataRepresentation>());

            var userModifiableTypes = modifiableTypes.Distinct(new ModifiableTypeDuplicateComparer());

            Assert.AreEqual(codeNamespace.Types.Count, userModifiableTypes.Count());

            // Modify types
            RemoveAttributes(codeNamespace);

            // Check for invalid characters in identifiers
            //CodeGenerator.ValidateIdentifiers(codeNamespace); // no implemented in Unity's Mono, has to be handwritten

            // output the C# code
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();

            StringBuilder code;
            using (StringWriter writer = new StringWriter())
            {
                codeProvider.GenerateCodeFromNamespace(codeNamespace, writer, new CodeGeneratorOptions());
                code = writer.GetStringBuilder();
                Debug.Log(code.ToString());
            }

            var filePath = Path.Combine(Application.dataPath, Path.Combine("XSD", "out.txt"));
            File.WriteAllText(filePath, code.ToString());

            // Refresh Unity to compile everything
            AssetDatabase.Refresh();
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

        // Remove all the attributes from each type in the CodeNamespace, except
        // System.Xml.Serialization.XmlTypeAttribute
        private static void RemoveAttributes(CodeNamespace codeNamespace)
        {
            foreach (CodeTypeDeclaration codeType in codeNamespace.Types)
            {
                CodeAttributeDeclaration xmlTypeAttribute = null;
                foreach (CodeAttributeDeclaration codeAttribute in codeType.CustomAttributes)
                {
                    if (codeAttribute.Name == "System.Xml.Serialization.XmlTypeAttribute")
                    {
                        xmlTypeAttribute = codeAttribute;
                    }
                }
                codeType.CustomAttributes.Clear();
                if (xmlTypeAttribute != null)
                {
                    codeType.CustomAttributes.Add(xmlTypeAttribute);
                }
            }
        }
    }
}