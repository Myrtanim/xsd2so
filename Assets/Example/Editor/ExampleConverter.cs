using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Xsd2So.Assets.Example.Editor
{
	static class ExampleLoader
	{
		[MenuItem("XSD/venetian_blind_test_1/Generate Code And Transfer Data", priority = 1)]
		public static void ConvertFixedAsset1()
		{
			// Generate Code
			var path = Application.dataPath + "/Example/VenetianBlindTest1/XData/venetian_blind_test_1.xsd";
			var xsd = File.ReadAllText(path);

			var config = new ConverterConfig();
			config.NamespaceXsdClasses = "Example.VenetianBlindTest1.Generated.Editor";
			config.NamespaceSoClasses = "Example.VenetianBlindTest1.Generated";
			config.XsdRootElementTypeName = "rootType";
			config.XmlRootNodeName = "root";
			config.SavePathXsdCode = PathCombine("Example", "VenetianBlindTest1", "Generated", "Editor", "XmlData_VenetianBlindTest1.cs");
			config.SavePathSoCode = PathCombine("Example", "VenetianBlindTest1", "Generated", "rootTypeSO.cs");

			Xsd2So.Generate(config, xsd);

			// Transfer Data XML -> SO
			var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Example/VenetianBlindTest1/XData/venetian_blind_test_1.xml");
			if (xmlText == null)
			{
				UnityEngine.Debug.LogError("Couldn't find XML 'Assets/Example/VenetianBlindTest1/XData/venetian_blind_test_1.xml'! Aborting.");
				return;
			}

			using (var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
			{
				xmlReader.Namespaces = true;

				var xmlClassesType = typeof(global::Example.VenetianBlindTest1.Generated.Editor.rootType);
				XmlSerializer serializer = new XmlSerializer(xmlClassesType);
				var xmlData = (global::Example.VenetianBlindTest1.Generated.Editor.rootType)serializer.Deserialize(xmlReader);

				var soInstance = ScriptableObject.CreateInstance<global::Example.VenetianBlindTest1.Generated.rootTypeSO>();

				xmlData.ToSerializable(soInstance);

				Directory.CreateDirectory("Assets/Example/VenetianBlindTest1/Resources");
				AssetDatabase.CreateAsset(soInstance, "Assets/Example/VenetianBlindTest1/Resources/rootSO1.asset");
				AssetDatabase.SaveAssets();
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = soInstance;
			}
		}

		[MenuItem("XSD/venetian_blind_test_1/Do loading test", priority = 2)]
		public static void LoadingPerformanceFixedAsset1()
		{
			LoadingPerformanceTest<
				global::Example.VenetianBlindTest1.Generated.Editor.rootType,
				global::Example.VenetianBlindTest1.Generated.rootTypeSO
			>("Assets/Example/VenetianBlindTest1/XData/venetian_blind_test_1.xml", "rootSO1");
		}

		[MenuItem("XSD/venetian_blind_test_2/Generate Code And Transfer Data", priority = 1)]
		public static void ConvertFixedAsset2()
		{
			// Generate Code
			var path = Application.dataPath + "/Example/VenetianBlindTest2/XData/venetian_blind_test_2.xsd";
			var xsd = File.ReadAllText(path);

			var config = new ConverterConfig();
			config.NamespaceXsdClasses = "Example.VenetianBlindTest2.Generated.Editor";
			config.NamespaceSoClasses = "Example.VenetianBlindTest2.Generated";
			config.XsdRootElementTypeName = "rootType";
			config.XmlRootNodeName = "root";
			config.SavePathXsdCode = PathCombine("Example", "VenetianBlindTest2", "Generated", "Editor", "XmlData_VenetianBlindTest2.cs");
			config.SavePathSoCode = PathCombine("Example", "VenetianBlindTest2", "Generated", "rootTypeSO.cs");

			Xsd2So.Generate(config, xsd);

			// Transfer Data XML -> SO
			var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Example/VenetianBlindTest2/XData/venetian_blind_test_2.xml");
			if (xmlText == null)
			{
				UnityEngine.Debug.LogError("Couldn't find XML 'Assets/Example/VenetianBlindTest2/XData/venetian_blind_test_2.xml'! Aborting.");
				return;
			}

			using (var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
			{
				xmlReader.Namespaces = true;

				var xmlClassesType = typeof(global::Example.VenetianBlindTest2.Generated.Editor.rootType);
				XmlSerializer serializer = new XmlSerializer(xmlClassesType);
				var xmlData = (global::Example.VenetianBlindTest2.Generated.Editor.rootType)serializer.Deserialize(xmlReader);

				var soInstance = ScriptableObject.CreateInstance<global::Example.VenetianBlindTest2.Generated.rootTypeSO>();

				xmlData.ToSerializable(soInstance);

				Directory.CreateDirectory("Assets/Example/VenetianBlindTest2/Resources");
				AssetDatabase.CreateAsset(soInstance, "Assets/Example/VenetianBlindTest2/Resources/rootSO2.asset");
				AssetDatabase.SaveAssets();
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = soInstance;
			}
		}

		[MenuItem("XSD/venetian_blind_test_2/Do loading test", priority = 2)]
		public static void LoadingPerformanceFixedAsset2()
		{
			LoadingPerformanceTest<
				global::Example.VenetianBlindTest2.Generated.Editor.rootType,
				global::Example.VenetianBlindTest2.Generated.rootTypeSO
			>("Assets/Example/VenetianBlindTest2/XData/venetian_blind_test_2.xml", "rootSO2");
		}

		private static void LoadingPerformanceTest<TXmlClass, TSoClass>(string xmlPath, string soPath) where TSoClass : ScriptableObject
		{
			// XML loading time test
			var stopwatch = new Stopwatch();

			stopwatch.Start();
			var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>(xmlPath);
			if (xmlText == null)
			{
				UnityEngine.Debug.LogError("Couldn't find XML '" + xmlPath + "'! Aborting.");
				stopwatch.Stop();
				return;
			}
			using (var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
			{
				xmlReader.Namespaces = true;

				var xmlClassesType = typeof(TXmlClass);
				XmlSerializer serializer = new XmlSerializer(xmlClassesType);
				var xmlData = (TXmlClass)serializer.Deserialize(xmlReader);
			}
			stopwatch.Stop();
			var elapsedXml = stopwatch.Elapsed;

			stopwatch.Reset();

			stopwatch.Start();
			var soData = Resources.Load<TSoClass>(soPath);
			stopwatch.Stop();
			var elapsedSo = stopwatch.Elapsed;

			var percentageSoFromXml = elapsedSo.TotalMilliseconds / elapsedXml.TotalMilliseconds * 100;
			UnityEngine.Debug.Log("Loading time XML vs generated SO:\n"
									+ "\t" + elapsedXml + " vs " + elapsedSo + " (SO parsing takes " + percentageSoFromXml.ToString("0.00000") + "% time of XML parsing)");
		}

		public static string PathCombine(params string[] pathElements)
		{
			return string.Join(Path.DirectorySeparatorChar.ToString(), pathElements);
		}
	}
}
