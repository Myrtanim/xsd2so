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

public class XSDConverter {

    [MenuItem("XSD/Convert")]
	public static void Convert()
    {
        var path = Application.dataPath + "/XSD/shared.xsd";
        StringBuilder console = new StringBuilder();

        var content = File.ReadAllText(path);

        XmlSchema xsd;
        using (var stream = new StringReader(content))
        {
            xsd = XmlSchema.Read(stream, null);
        }

        console.AppendLine("xsd.IsCompiled = " + xsd.IsCompiled + "\n" + xsd.LineNumber + ", " + xsd.Version);

        XmlSchemas xsds = new XmlSchemas();
        xsds.Add(xsd);
        xsds.Compile(null, true);
        XmlSchemaImporter schemaImporter = new XmlSchemaImporter(xsds);

        // create the codedom
        CodeNamespace codeNamespace = new CodeNamespace("Generated");
        XmlCodeExporter codeExporter = new XmlCodeExporter(codeNamespace);

        List<XmlTypeMapping> maps = new List<XmlTypeMapping>();
        foreach (XmlSchemaType schemaType in xsd.SchemaTypes.Values)
        {
            maps.Add(schemaImporter.ImportSchemaType(schemaType.QualifiedName));
        }
        foreach (XmlSchemaElement schemaElement in xsd.Elements.Values)
        {
            maps.Add(schemaImporter.ImportTypeMapping(schemaElement.QualifiedName));
        }
        foreach (XmlTypeMapping map in maps)
        {
            codeExporter.ExportTypeMapping(map);
        }

        console.AppendLine().AppendLine("#########################################").AppendLine();

        RemoveAttributes(codeNamespace, console);

        console.AppendLine().AppendLine("#########################################").AppendLine();

        // Check for invalid characters in identifiers
        //CodeGenerator.ValidateIdentifiers(codeNamespace);

        // output the C# code
        CSharpCodeProvider codeProvider = new CSharpCodeProvider();

        using (StringWriter writer = new StringWriter())
        {
            codeProvider.GenerateCodeFromNamespace(codeNamespace, writer, new CodeGeneratorOptions());
            console.AppendLine(writer.GetStringBuilder().ToString());
        }

        var filePath = Path.Combine(Application.dataPath, Path.Combine("XSD", "out.txt"));
        File.WriteAllText(filePath, console.ToString());

        AssetDatabase.Refresh();
    }

    // Remove all the attributes from each type in the CodeNamespace, except
    // System.Xml.Serialization.XmlTypeAttribute
    private static void RemoveAttributes(CodeNamespace codeNamespace, StringBuilder console)
    {
        foreach (CodeTypeDeclaration codeType in codeNamespace.Types)
        {
            console.AppendLine(codeType.Name);

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
