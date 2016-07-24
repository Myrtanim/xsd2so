# Xsd2So #

This provides a tool to generate Unity ScriptableObjects from an XSD and also provides parsing capabilities, so you can easily parse the content of a matching XML into the ScriptableObject.

So this projects tries to fuse XML data binding with Unity's native serialization capabilities.

### What is this repository for? ###

* generate code from XSD to Unity's ScriptableObject
* also generates parsing of XML data to a ScriptableObject instance
* 0.1.0

### How do I get set up? ###

* Checkout
* Run with Unity 5.3.5 or newer
* Go

### Contribution guidelines ###

* TBD

### Who do I talk to? ###

* Repo owner

### Performance measurements ###
This whole project has the main intention to utilize the performance-optimized data structures from Unity.
To proof that the whole thing is worth it, this repo contains some loading time tests.

Some results are the following.

Test machine is a PC with

* Windows 10 64bit
* 8 GB DDR3 1600 Mhz RAM
* Intel Core i5-4690K @ 3.5 Ghz
* data loaded from a 1TB HDD

Each measurement was done 30 times and min/max/average values where calculated from `StopWatch` data.

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