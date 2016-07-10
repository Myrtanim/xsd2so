using System;
using System.CodeDom;
using System.IO;

namespace Xsd2So
{
	internal struct Pair<T1, T2>
	{
		public T1 Left { get; set; }
		public T2 Right { get; set; }

		public Pair(T1 left, T2 right)
		{
			Left = left;
			Right = right;
		}
	}

	static class CodeDomHelper
	{
		public static void AddAttribute(CodeTypeDeclaration codeDomType, Type attributeType, params Pair<string, object>[] attributeArgumentPairs)
		{
			var attributeDecl = new CodeAttributeDeclaration(new CodeTypeReference(attributeType));

			foreach (var p in attributeArgumentPairs)
			{
				var attr = new CodeAttributeArgument() {
					Name = p.Left,
					Value = new CodePrimitiveExpression(p.Right)
				};
				attributeDecl.Arguments.Add(attr);
			}
			codeDomType.CustomAttributes.Add(attributeDecl);
		}
	}

	static class StringHelper
	{
		public static string ToFirstLetterLowerCase(this string str)
		{
			return Char.ToLowerInvariant(str[0]) + str.Substring(1);
		}

		public static string PathCombine(bool prepandDirSeparator, params string[] pathElements)
		{
			return (prepandDirSeparator ? Path.DirectorySeparatorChar.ToString() : "")
					+ string.Join(Path.DirectorySeparatorChar.ToString(), pathElements);
		}
	}
}
