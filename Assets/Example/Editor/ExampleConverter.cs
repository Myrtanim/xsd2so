//using Example.Generated;
//using Example.Generated.Editor;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Xsd2So.Assets.Example.Editor
{
	static class ExampleLoader
	{
		[MenuItem("XSD/Example/Load and transfer data")]
		public static void LoadAndConvert()
		{
			//var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Example/XSD/test.xml");

			//using(var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
			//{
			//	xmlReader.Namespaces = true;

			//	XmlSerializer serializer = new XmlSerializer(typeof(XsdClass));
			//	XsdClass xmlData = (XsdClass)serializer.Deserialize(xmlReader);

			//	var soInstance = ScriptableObject.CreateInstance<SOClass>();

			//	xmlData.ToSerializable(soInstance);

			//	Directory.CreateDirectory("Assets/Resources/XSD");
			//	AssetDatabase.CreateAsset(soInstance, "Assets/Resources/XSD/testSO.asset");
			//	AssetDatabase.SaveAssets();
			//	EditorUtility.FocusProjectWindow();
			//	Selection.activeObject = soInstance;
			//}
		}
	}
}
