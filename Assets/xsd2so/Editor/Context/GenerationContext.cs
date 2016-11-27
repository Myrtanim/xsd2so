using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace Xsd2So
{
    class GenerationContext
    {
        /// <summary>
        /// The configuration for the code generator to use.
        /// </summary>
        public ConverterConfig Config { get; internal set; }

        public XmlSchema XsdSchema { get; internal set; }
        public XmlSchemas XsdSchemas { get; internal set; }

        /// <summary>
        /// The code generated from the XSD.
        /// </summary>
        public CodeNamespace XmlCode { get; internal set; }

        /// <summary>
        /// The association of XSD Schema Type and XSD Code Type.
        /// </summary>
        public IEnumerable<DataRepresentation> XsdCodeMapping { get; internal set; }

        /// <summary>
        /// Contains the code generated for the ScriptableObject.
        /// </summary>
        public CodeNamespace ScriptableObjectCode { get; internal set; }

        public GenerationContext(ConverterConfig config)
        {
            Config = config;

            XmlCode = new CodeNamespace(config.NamespaceXsdClasses);
            ScriptableObjectCode = new CodeNamespace(config.NamespaceSoClasses);

            XsdCodeMapping = new List<DataRepresentation>();

            CheckSoFileName(config.XsdRootElementTypeName, config.SavePathSoCode);
        }

        private void CheckSoFileName(string xmlRootNodeName, string savePathSoCode)
        {
            var advisedSoFileName = xmlRootNodeName + "SO.cs";
            var actualSoFileName = Path.GetFileName(savePathSoCode);
            if (advisedSoFileName != actualSoFileName)
            {
                Debug.LogWarning("You specified the file name '" + actualSoFileName + "' for the ScriptableObject, "
                                 +
                                 "but its advised to use the name of the scriptable object as the file name (in this case '" +
                                 advisedSoFileName + "')\n"
                                 +
                                 "You can keep this file name, but Unity's Inspector will have problems rendering the scriptable object.");
            }
        }
    }
}