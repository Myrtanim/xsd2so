namespace Xsd2So
{
	public class ConverterConfig
	{
		public string NamespaceXsdClasses { get; set; }
		public string NamespaceSoClasses { get; set; }

		public string XsdRootElementTypeName { get; set; }
		public string XmlRootNodeName { get; set; }

		public string SavePathXsdCode { get; internal set; }
		public string SavePathSoCode { get; internal set; }

		public string SoSuffix { get { return "SO"; } }
	}
}
