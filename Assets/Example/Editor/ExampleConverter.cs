using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Xsd2So.Assets.Example.Editor
{
	static class ExampleLoader
	{
		[MenuItem("XSD/venetian_blind_test_1/Generate Code", priority = 1)]
		public static void ConvertFixedAsset()
		{
			// Load XSD as text file
			var path = Application.dataPath + "/Example/XData/venetian_blind_test_1.xsd";
			var xsd = File.ReadAllText(path);

			var config = new ConverterConfig();
			config.NamespaceXsdClasses = "Example.VenetianBlindTest1.Generated.Editor";
			config.NamespaceSoClasses = "Example.VenetianBlindTest1.Generated";
			config.XsdRootElementTypeName = "rootType";
			config.XmlRootNodeName = "root";
			config.SavePathXsdCode = PathCombine("Example", "VenetianBlindTest1", "Generated", "Editor", "XmlData_VenetianBlindTest1.cs");
			config.SavePathSoCode = PathCombine("Example", "VenetianBlindTest1", "Generated", "rootTypeSO.cs");

			Xsd2So.Generate(config, xsd);
		}

		public static string PathCombine(params string[] pathElements)
		{
			return string.Join(Path.DirectorySeparatorChar.ToString(), pathElements);
		}

		[MenuItem("XSD/venetian_blind_test_1/Load and transfer data", priority = 2)]
		public static void LoadAndConvert()
		{
			var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Example/XData/venetian_blind_test_1.xml");

			if (xmlText == null)
			{
				Debug.LogError("Couldn't find XML 'Assets/Example/XData/venetian_blind_test_1.xml'! Aborting.");
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
				AssetDatabase.CreateAsset(soInstance, "Assets/Example/VenetianBlindTest1/Resources/rootSO.asset");
				AssetDatabase.SaveAssets();
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = soInstance;
			}
		}
	}
}
