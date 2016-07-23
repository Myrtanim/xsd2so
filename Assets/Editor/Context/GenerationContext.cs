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
		public ConverterConfig Config { get; internal set; }

		public XmlSchema XsdSchema { get; internal set; }
		public XmlSchemas XsdSchemas { get; internal set; }

		public CodeNamespace XmlCode { get; internal set; }

        public IEnumerable<DataRepresentation> XsdCodeMapping { get; internal set; }

        public CodeNamespace ScriptableObjectCode { get; internal set; }

        public CodeNamespaceCollection AdditionalCode { get; internal set; }

		public GenerationContext(ConverterConfig config)
        {
			Config = config;

            XmlCode = new CodeNamespace(config.NamespaceXsdClasses);
            ScriptableObjectCode = new CodeNamespace(config.NamespaceSoClasses);

            XsdCodeMapping = new List<DataRepresentation>();
            AdditionalCode = new CodeNamespaceCollection();

			CheckSoFileName(config.XsdRootElementTypeName, config.SavePathSoCode);
		}

		private void CheckSoFileName(string xmlRootNodeName, string savePathSoCode)
		{
			var advisedSoFileName = xmlRootNodeName + "SO.cs";
			var actualSoFileName = Path.GetFileName(savePathSoCode);
			if (advisedSoFileName != actualSoFileName)
			{
				Debug.LogWarning("You specified the file name '" + actualSoFileName + "' for the ScriptableObject, "
					+ "but its advised to use the name of the scriptable object as the file name (in this case '" + advisedSoFileName + "')\n"
					+ "You can keep this file name, but Unity's Inspector will have problems rendering the scriptable object.");
			}
		}
	}
}
