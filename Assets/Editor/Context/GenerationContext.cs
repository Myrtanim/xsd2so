using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Xsd2So
{
    class GenerationContext
    {
        public XsdConfig XsdConfig { get; set; }

        public CodeNamespace XmlCode { get; set; }

        public IEnumerable<DataRepresentation> XsdCodeMapping { get; set; }

        public CodeNamespace ScriptableObjectCode { get; set; }

        public CodeNamespaceCollection AdditionalCode { get; set; }

        public GenerationContext(string xmlCodeNamespaceName, string scriptableObjectNamespaceName)
        {
            XsdConfig = new XsdConfig();

            XmlCode = new CodeNamespace(xmlCodeNamespaceName);
            ScriptableObjectCode = new CodeNamespace(scriptableObjectNamespaceName);

            XsdCodeMapping = new List<DataRepresentation>();
            AdditionalCode = new CodeNamespaceCollection();
        }
    }

    class XsdConfig
    {
        public XmlSchema XsdSchema { get; internal set; }

        public XmlSchemas XsdSchemas { get; private set; }

        internal void ProcessXsd(string content)
        {
            // Parse Xsd to schema code representation
            using (var stream = new StringReader(content))
            {
                XsdSchema = XmlSchema.Read(stream, null);
            }

            // Compile Xsd
            XsdSchemas = new XmlSchemas();
            XsdSchemas.Add(XsdSchema);
            XsdSchemas.Compile(null, true);
        }
    }
}
