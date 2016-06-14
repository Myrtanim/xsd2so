﻿using System;
using System.CodeDom;

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
}