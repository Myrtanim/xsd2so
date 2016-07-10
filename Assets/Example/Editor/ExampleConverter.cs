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
			var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Example/XSD/test.xml");

			using(var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
			{
				xmlReader.Namespaces = true;

				//XmlSerializer serializer = new XmlSerializer(typeof(BalancingData));
				//BalancingData xmlData = (BalancingData)serializer.Deserialize(xmlReader);

				//var soInstance = ScriptableObject.CreateInstance<BalancingDataSO>();

				//xmlData.ToSerializable(soInstance);

				//AssetDatabase.CreateAsset(soInstance, "Resources/XSD/testSO.asset");
				//AssetDatabase.SaveAssets();
				//EditorUtility.FocusProjectWindow();
				//Selection.activeObject = soInstance;
			}
		}
	}
}
