namespace Xsd2So
{
	public class ConverterConfig
	{
	    /// <summary>
	    /// The relative base path where additional XSD files should be loaded from.<br/>
	    /// This path is relative to the Unity project root directory (the folder containing Assets, Library and so on).
	    /// <br/><br/>
	    /// This is only necessary if you are using &lt;xs:include ...&gt; in you XSDs.
	    /// </summary>
	    public string XsdSearchPath { get; set; }

	    public string NamespaceXsdClasses { get; set; }
		public string NamespaceSoClasses { get; set; }

		public string XsdRootElementTypeName { get; set; }
		public string XmlRootNodeName { get; set; }

		public string SavePathXsdCode { get; internal set; }
		public string SavePathSoCode { get; internal set; }

		public string SoSuffix { get { return "SO"; } }

	    public ConverterConfig()
	    {
	        XsdSearchPath = "Assets";
	    }
	}
}
