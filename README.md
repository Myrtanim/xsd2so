# Xsd2So #

`xsd2so` provides a tool to generate [Unity ScriptableObject](http://docs.unity3d.com/Manual/class-ScriptableObject.html)s (SO) from an XSD and also provides parsing capabilities, so you can easily parse the content of a matching XML into the SO.

So this projects fuses [XML data binding](https://en.wikipedia.org/wiki/XML_data_binding) with Unity's native serialization capabilities.

The idea came from being unsatisfied by the performance (parsing times and file size) of an existing XML-based workflow to handle balancing data for a commercial game. Changing the XML-based workflow was not an option because too many other tools and people were involved.

### What is this repository for? ###

* generate C# code from XSD
* generate matching SO code (in C#)
* also generates parsing of XML data to a SO instance

### How do I get set up? ###

* Download the most recent [Unity package](https://bitbucket.org/Myrtanim/xsd2so/downloads) from this repository
* Import the package in your project
* create an editor script to use `xsd2So`, see [`ExampleConverter.ConvertFixedAsset1()`](https://github.com/Myrtanim/xsd2so/blob/master/Assets/Example/Editor/ExampleConverter.cs) for more details

or

* clone the repository
* open in Unity 5.3.4 or any Unity 5.4.x

**Hint**
This tool has not been tested with any Unity 20xx versions and will probably not work, especially since the switch away from the custom Mono implementation.

### Contribution guidelines ###

**If you have an idea or want to suggest an improvement:**

Either

* fork, modify an create a pull request.

or

* create an [issue](https://github.com/Myrtanim/xsd2so/issues)

**If you found a bug:**

Please create an [issue](https://github.com/Myrtanim/xsd2so/issues) with a minimal example XSD/XML and a description of the issue.

I try to answer it as soon as possible, but it can still take a while.

### Who do I talk to? ###

* [Myrtanim](https://github.com/Myrtanim/)

### License ###

`xsd2So` is released under an MIT/X11 license; see the LICENSE file.

### Performance measurements ###
This whole project has the main intention to utilize the performance-optimized deserialization from Unity while not changing an existing XML-based workflow.
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