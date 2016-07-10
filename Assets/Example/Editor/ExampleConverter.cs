using UnityEditor;
using UnityEngine;

namespace Xsd2So.Assets.Example.Editor
{
	class ExampleLoader
	{
		[MenuItem("XSD/Example/Load and transfer data")]
		public void LoadAndConvert()
		{
			var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Example/XSD/test.xml");
		}
	}
}
