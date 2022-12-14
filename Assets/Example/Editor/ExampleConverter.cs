using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            // == Generate the code from the XSD =====
            // Read in the XSD file
            var path = Application.dataPath + "/Example/VenetianBlindTest1/XData/test.xsd";
            var xsd = File.ReadAllText(path);

            // Set up configuration.
            var config = new ConverterConfig()
            {
                NamespaceXsdClasses = "Example.VenetianBlindTest1.Generated.Editor",
                NamespaceSoClasses = "Example.VenetianBlindTest1.Generated",
                XsdRootElementTypeName = "rootType",
                XmlRootNodeName = "root",
                // the following paths are relative to your Assets folder!
                SavePathXsdCode = "Example/VenetianBlindTest1/Generated/Editor/XmlData_VenetianBlindTest1.cs",
                SavePathSoCode = "Example/VenetianBlindTest1/Generated/rootTypeSO.cs"
            };

            // Generate code.
            // The result is directly saved to file. See config.SavePathXsdCode and config.SavePathSoCode.
            Xsd2So.Generate(config, xsd);

            // == Transfer data from XML to SO =====
            // Load the XML as a text asset. You can also use File.ReadAllText(...).
            var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Example/VenetianBlindTest1/XData/test.xml");
            if (xmlText == null)
            {
                UnityEngine.Debug.LogError(
                    "Couldn't find XML 'Assets/Example/VenetianBlindTest1/XData/test.xml'! Aborting.");
                return;
            }

            // Read in XML.
            using (var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
            {
                xmlReader.Namespaces = true;

                // Convert the XML content to a C# object. This C# object is the XML code generated from the Xsd2So.
                var xmlClassesType = typeof(global::Example.VenetianBlindTest1.Generated.Editor.rootType);
                XmlSerializer serializer = new XmlSerializer(xmlClassesType);
                var xmlData =
                    (global::Example.VenetianBlindTest1.Generated.Editor.rootType) serializer.Deserialize(xmlReader);

                // Create the matching ScriptableObject object. This SO object is also an instance of the SO code generated from Xsd2So.
                var soInstance =
                    ScriptableObject.CreateInstance<global::Example.VenetianBlindTest1.Generated.rootTypeSO>();

                // Copy all data from the XML object to the SO object. The generated code takes care of all the details.
                xmlData.ToSerializable(soInstance);

                // Finally, save the SO object as an asset and select it in Unity.
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
            MultiLoadingPerformanceTest<
                global::Example.VenetianBlindTest1.Generated.Editor.rootType,
                global::Example.VenetianBlindTest1.Generated.rootTypeSO
            >(30, "Assets/Example/VenetianBlindTest1/XData/test.xml", "rootSO1");
        }

        [MenuItem("XSD/venetian_blind_test_2/Generate Code And Transfer Data", priority = 1)]
        public static void ConvertFixedAsset2()
        {
            // Generate Code
            var path = Application.dataPath + "/Example/VenetianBlindTest2/XData/test.xsd";
            var xsd = File.ReadAllText(path);

            var config = new ConverterConfig()
            {
                NamespaceXsdClasses = "Example.VenetianBlindTest2.Generated.Editor",
                NamespaceSoClasses = "Example.VenetianBlindTest2.Generated",
                XsdRootElementTypeName = "rootType",
                XmlRootNodeName = "root",
                SavePathXsdCode = "Example/VenetianBlindTest2/Generated/Editor/XmlData_VenetianBlindTest2.cs",
                SavePathSoCode = "Example/VenetianBlindTest2/Generated/rootTypeSO.cs"
            };

            Xsd2So.Generate(config, xsd);

            // Transfer Data XML -> SO
            var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Example/VenetianBlindTest2/XData/test.xml");
            if (xmlText == null)
            {
                UnityEngine.Debug.LogError(
                    "Couldn't find XML 'Assets/Example/VenetianBlindTest2/XData/test.xml'! Aborting.");
                return;
            }

            using (var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
            {
                xmlReader.Namespaces = true;

                var xmlClassesType = typeof(global::Example.VenetianBlindTest2.Generated.Editor.rootType);
                XmlSerializer serializer = new XmlSerializer(xmlClassesType);
                var xmlData =
                    (global::Example.VenetianBlindTest2.Generated.Editor.rootType) serializer.Deserialize(xmlReader);

                var soInstance =
                    ScriptableObject.CreateInstance<global::Example.VenetianBlindTest2.Generated.rootTypeSO>();

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
            MultiLoadingPerformanceTest<
                global::Example.VenetianBlindTest2.Generated.Editor.rootType,
                global::Example.VenetianBlindTest2.Generated.rootTypeSO
            >(30, "Assets/Example/VenetianBlindTest2/XData/test.xml", "rootSO2");
        }

        [MenuItem("XSD/Included XSD/Generate Code And Transfer Data", priority = 3)]
        public static void ConvertIncludedXsd()
        {
            // == Generate the code from the XSD =====
            // Read in the XSD file
            var path = Application.dataPath + "/Example/IncludedXSD/XData/main.xsd";
            var xsd = File.ReadAllText(path);

            // Set up configuration.
            var config = new ConverterConfig()
            {
                XsdSearchPath = "Assets/Example/IncludedXSD/XData",
                NamespaceXsdClasses = "Example.IncludedXsd.Generated.Editor",
                NamespaceSoClasses = "Example.IncludedXsd.Generated",
                XsdRootElementTypeName = "buildingsType",
                XmlRootNodeName = "buildings",
                // the following paths are relative to your Assets folder!
                SavePathXsdCode = "Example/IncludedXSD/Generated/Editor/XmlData_IncludedXSD.cs",
                SavePathSoCode = "Example/IncludedXSD/Generated/buildingsTypeSO.cs"
            };

            // Generate code.
            // The result is directly saved to file. See config.SavePathXsdCode and config.SavePathSoCode.
            Xsd2So.Generate(config, xsd);

            // == Transfer data from XML to SO =====
            // Load the XML as a text asset. You can also use File.ReadAllText(...).
            var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Example/IncludedXSD/XData/data.xml");
            if (xmlText == null)
            {
                UnityEngine.Debug.LogError("Couldn't find XML 'Assets/Example/IncludedXSD/XData/data.xml'! Aborting.");
                return;
            }

            // Read in XML.
            using (var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
            {
                xmlReader.Namespaces = true;

                // Convert the XML content to a C# object. This C# object is the XML code generated from the Xsd2So.
                var xmlClassesType = typeof(global::Example.IncludedXsd.Generated.Editor.buildingsType);
                XmlSerializer serializer = new XmlSerializer(xmlClassesType);
                var xmlData =
                    (global::Example.IncludedXsd.Generated.Editor.buildingsType) serializer.Deserialize(xmlReader);

                // Create the matching ScriptableObject object. This SO object is also an instance of the SO code generated from Xsd2So.
                var soInstance =
                    ScriptableObject.CreateInstance<global::Example.IncludedXsd.Generated.buildingsTypeSO>();

                // Copy all data from the XML object to the SO object. The generated code takes care of all the details.
                xmlData.ToSerializable(soInstance);

                // Finally, save the SO object as an asset and select it in Unity.
                Directory.CreateDirectory("Assets/Example/IncludedXSD/Resources");
                AssetDatabase.CreateAsset(soInstance, "Assets/Example/IncludedXSD/Resources/buildingsSO.asset");
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = soInstance;
            }
        }

        private static void MultiLoadingPerformanceTest<TXmlClass, TSoClass>(
            int iterations,
            string xmlPath, string soPath)
            where TSoClass : ScriptableObject
        {
            List<TimeSpan> allXmlTimes = new List<TimeSpan>(iterations);
            List<TimeSpan> allSoTimes = new List<TimeSpan>(iterations);

            for (int i = 0; i < iterations; i++)
            {
                var abort = EditorUtility.DisplayCancelableProgressBar("Measuring...",
                    "Iteration " + i + "/" + iterations, (float) i / iterations);
                if (abort)
                {
                    break;
                }

                TimeSpan xmlTime, soTime;
                LoadingPerformanceTest<
                    TXmlClass,
                    TSoClass
                >(xmlPath, soPath, out xmlTime, out soTime);

                allXmlTimes.Add(xmlTime);
                allSoTimes.Add(soTime);
            }

            var avgXmlTime = GetAverageTime(allXmlTimes);
            var minXmlTime = new TimeSpan(allXmlTimes.Min(timeSpan => timeSpan.Ticks));
            var maxXmlTime = new TimeSpan(allXmlTimes.Max(timeSpan => timeSpan.Ticks));

            var avgSoTime = GetAverageTime(allSoTimes);
            var minSoTime = new TimeSpan(allSoTimes.Min(timeSpan => timeSpan.Ticks));
            var maxSoTime = new TimeSpan(allSoTimes.Max(timeSpan => timeSpan.Ticks));

            UnityEngine.Debug.Log("Format |      min         |        max       |       avg"
                                  + "\nXML    | " + minXmlTime + " | " + maxXmlTime + " | " + avgXmlTime
                                  + "\nSO     | " + minSoTime + " | " + maxSoTime + " | " + avgSoTime);

            EditorUtility.ClearProgressBar();
        }

        private static void LoadingPerformanceTest<TXmlClass, TSoClass>(
            string xmlPath, string soPath,
            out TimeSpan xmlTime, out TimeSpan soTime)
            where TSoClass : ScriptableObject
        {
            // XML loading time test
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var xmlText = AssetDatabase.LoadAssetAtPath<TextAsset>(xmlPath);
            if (xmlText == null)
            {
                UnityEngine.Debug.LogError("Couldn't find XML '" + xmlPath + "'! Aborting.");
                stopwatch.Stop();

                xmlTime = TimeSpan.Zero;
                soTime = TimeSpan.Zero;

                return;
            }
            using (var xmlReader = new XmlTextReader(new StringReader(xmlText.text)))
            {
                xmlReader.Namespaces = true;

                var xmlClassesType = typeof(TXmlClass);
                XmlSerializer serializer = new XmlSerializer(xmlClassesType);
                var xmlData = (TXmlClass) serializer.Deserialize(xmlReader);
            }
            stopwatch.Stop();
            xmlTime = stopwatch.Elapsed;

            stopwatch.Reset();

            stopwatch.Start();
            var soData = Resources.Load<TSoClass>(soPath);
            stopwatch.Stop();
            soTime = stopwatch.Elapsed;

            Resources.UnloadAsset(soData);
            Resources.UnloadAsset(xmlText);
            xmlText = null;
            soData = null;

            Resources.UnloadUnusedAssets();
            GC.Collect();
            GC.Collect();
            GC.Collect();
        }

        private static TimeSpan GetAverageTime(List<TimeSpan> allXmlTimes)
        {
            var averageXmlTime = allXmlTimes.Average(timeSpan => timeSpan.Ticks);
            return new TimeSpan(Convert.ToInt64(averageXmlTime));
        }
    }
}