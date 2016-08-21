# Xsd2So #

This provides a tool to generate [Unity ScriptableObjects](http://docs.unity3d.com/Manual/class-ScriptableObject.html) (SO) from an XSD and also provides parsing capabilities, so you can easily parse the content of a matching XML into the SO.

So this projects fuses [XML data binding](https://en.wikipedia.org/wiki/XML_data_binding) with Unity's native serialization capabilities.

### What is this repository for? ###

* generate C# code from XSD
* generate matching SO code (in C#)
* also generates parsing of XML data to a SO instance

### How do I get set up? ###

* Download the [Unity package](https://bitbucket.org/Myrtanim/xsd2so/downloads/xsd2so.unitypackage) from this repository
* Import the package in your project
* create an editor script to use Xsd2So, see [`ExampleConverter.ConvertFixedAsset1()`](https://bitbucket.org/Myrtanim/xsd2so/src/ffd2c0d59dafabec3d26dc55010d2ceec0f91dbf/Assets/Example/Editor/ExampleConverter.cs?at=default&fileviewer=file-view-default) for more details

or

* clone the repository
* open in Unity 5.3.4 or later

### Contribution guidelines ###

**If you have an idea or want to suggest an improvement:**

Either

* fork, modify an create a pull request.

or

* create an [issue](https://bitbucket.org/Myrtanim/xsd2so/issues)

**If you found a bug:**

Please create an [issue](https://bitbucket.org/Myrtanim/xsd2so/issues) with a minimal example XSD/XML and a description of the issue.

I try to answer it as soon as possible, but it can still take a while.

### Who do I talk to? ###

* [Myrtanim](https://bitbucket.org/Myrtanim/)

### License ###

Xsd2So is released under an MIT/X11 license; see the LICENSE file.

### Performance measurements ###
This whole project has the main intention to utilize the performance-optimized deserialization from Unity.
To proof that the whole thing is worth it, this repo contains some loading time tests, see the XSD menu of this project in Unity.

Some results are the following. The test machine is a PC with

* Windows 10 64bit
* 8 GB DDR3 1600 Mhz RAM
* Intel Core i5-4690K @ 3.5 Ghz
* data loaded from a 1TB HDD

Each measurement was done 30 times and min/max/average values where calculated from `Stopwatch` data.

# Small example (1.76kb XML vs 4.47kb SO, same data content) #
| Format |        min         |         max        |        avg         |
| ------ | ------------------ | ------------------ | ------------------ |
| XML    | `00:00:00.0017389` | `00:00:00.0136228` | `00:00:00.0022552` |
| SO     | `00:00:00.0000866` | `00:00:00.0004697` | `00:00:00.0001085` |

# Big example (5.24MB XML vs 1.45MB SO, same data content) #
| Format |        min         |         max        |        avg         |
| ------ | ------------------ | ------------------ | ------------------ |
| XML    | `00:00:01.4400673` | `00:00:01.4848626` | `00:00:01.4504397` |
| SO     | `00:00:00.0000170` | `00:00:00.1129411` | `00:00:00.1057949` |