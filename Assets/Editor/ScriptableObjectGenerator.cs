using System;
using System.CodeDom;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Xsd2So
{
    internal class ScriptableObjectGenerator : ICodeModifier
    {
		private string soClassPostfix;

		public ScriptableObjectGenerator(string soClassPostfix)
		{
			this.soClassPostfix = soClassPostfix;
		}

		public void Execute(GenerationContext ctx)
		{
			Dictionary<string, CodeTypeDeclaration> xmlTypeToSoType = new Dictionary<string, CodeTypeDeclaration>();

			// Step 1:
			// Generate all SO types without their members, so that we have all required types available.
			foreach (var xsdType in ctx.XsdCodeMapping)
			{
				var copiedType = CreateSoType(xsdType.CodeType, ctx);
				xmlTypeToSoType.Add(xsdType.CodeType.Name, copiedType);

				if (copiedType.IsEnum)
				{
					ctx.ScriptableObjectCode.Types.Add(copiedType);
				}
			}

			// Step 2:
			// Generate members of SO types and replace XSD member types with their SO equivalents.
            foreach (var xsdType in ctx.XsdCodeMapping)
            {
				if (!xsdType.CodeType.IsEnum)
				{
					var soType = CopyMembers(xsdType.CodeType, ctx, xmlTypeToSoType);
					CreateCopyMethod(soType, xsdType.CodeType, xmlTypeToSoType, ctx);

					ctx.ScriptableObjectCode.Types.Add(soType);
				}
			}
		}

		#region Code generation for data copy method XSD -> SO

		private void CreateCopyMethod(
			CodeTypeDeclaration soType,
			CodeTypeDeclaration xsdType,
			Dictionary<string, CodeTypeDeclaration> xmlTypeToSoType,
			GenerationContext ctx
		)
		{
			// create ToSerializable method for this xsdType
			var toSerializableMethod = new CodeMemberMethod();
			toSerializableMethod.Name = "ToSerializable";
			toSerializableMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			toSerializableMethod.ReturnType = new CodeTypeReference(typeof(void));

			var paramType = ctx.ScriptableObjectCode.Name + "." + soType.Name;
			var paramName = soType.Name.ToFirstLetterLowerCase();
			var parameter = new CodeParameterDeclarationExpression(paramType, paramName);
			toSerializableMethod.Parameters.Add(parameter);

			foreach (CodeTypeMember member in xsdType.Members)
			{
				if (member is CodeMemberProperty)
				{
					var xsdProperty = member as CodeMemberProperty;
					var xsdPropertyType = xsdProperty.Type.BaseType;

					bool isXsdType = xmlTypeToSoType.ContainsKey(xsdPropertyType);

					CodeStatement transferStatement = null;
					if (isXsdType)
					{
						Debug.Log(xsdType.Name + "." + xsdProperty.Name + " : " + xsdPropertyType + " (<b>XSD</b> type)");
						var memberXsdType = ctx.XsdCodeMapping.First(ele => ele.CodeType.Name == xsdPropertyType);
						if (memberXsdType == null)
						{
							Debug.LogError("No Xsd Code mapping founs for '" + xsdPropertyType + "'");
							continue;
						}

						var memberXsdCodeDeclaration = memberXsdType.CodeType;
						if (memberXsdCodeDeclaration.IsEnum) // handle enum members with Xsd types
						{
							var targetEnumTypeFQN = ctx.ScriptableObjectCode.Name + "." + xmlTypeToSoType[memberXsdCodeDeclaration.Name].Name;
							transferStatement = new CodeAssignStatement(
								new CodePropertyReferenceExpression(
									new CodeVariableReferenceExpression(paramName),
									xsdProperty.Name.ToFirstLetterLowerCase()
								),
								new CodeCastExpression(
									new CodeTypeReference(targetEnumTypeFQN),
									new CodePropertyReferenceExpression(
										new CodeThisReferenceExpression(),
										xsdProperty.Name
									)
								)
							);
						}
						// handle normal members with Xsd types

						// handle array members with Xsd types
					}
					else
					{
						// soObject.value = this.value;
						transferStatement = new CodeAssignStatement(
								new CodePropertyReferenceExpression(
									new CodeVariableReferenceExpression(paramName),
									xsdProperty.Name.ToFirstLetterLowerCase()
								),
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(),
									xsdProperty.Name
								)
							);
					}

					if (transferStatement != null)
					{
						toSerializableMethod.Statements.Add(transferStatement);
					}
				}
			}

			xsdType.Members.Add(toSerializableMethod);
		}

		#endregion

		#region Type creation methods

		private CodeTypeDeclaration CreateSoType(CodeTypeDeclaration codeType, GenerationContext context)
		{
			if (codeType.IsClass)
			{
				if (codeType.Name == context.RootElementTypeName)
				{
					return CreateSoRootClass(codeType);
				}
				else
				{
					return CreateSoClass(codeType);
				}
			}
			else if (codeType.IsEnum)
			{
				return CopyEnum(codeType);
			}
			else
			{
				throw new ArgumentException("Unhandled code type: " + codeType.Name);
			}
		}

		private CodeTypeDeclaration CreateSoRootClass(CodeTypeDeclaration codeType)
		{
			var rootClass = CreateSoClass(codeType);
			rootClass.Name += soClassPostfix;
			rootClass.CustomAttributes.Clear();
			rootClass.BaseTypes.Add(new CodeTypeReference(typeof(ScriptableObject)));

			// TODO: this is optional and does not necessarily have to be, since the
			// xsdType.ToSerializable() exists...
			CodeDomHelper.AddAttribute(rootClass,
				typeof(CreateAssetMenuAttribute),
				new Pair<string, object>("fileName", rootClass.Name),
				new Pair<string, object>("menuName", "Xsd2So/Create " + rootClass.Name),
				new Pair<string, object>("order", 1)
			);

			return rootClass;
		}

		private CodeTypeDeclaration CreateSoClass(CodeTypeDeclaration codeType)
		{
			var r = new CodeTypeDeclaration(codeType.Name);
			r.IsClass = true; // make it a class
			r.IsPartial = true; // make it a partial class

			// Add Serializable attribute
			CodeDomHelper.AddAttribute(r, typeof(SerializableAttribute));

			return r;
		}

		private CodeTypeDeclaration CopyEnum(CodeTypeDeclaration codeType)
		{
			var r = new CodeTypeDeclaration(codeType.Name);
			r.IsEnum = true;

			foreach (CodeMemberField member in codeType.Members)
			{
				var enumVal = new CodeMemberField(member.Type, member.Name);
				r.Members.Add(enumVal);
			}

			return r;
		}

		private CodeTypeDeclaration CopyMembers(CodeTypeDeclaration codeType, GenerationContext context, Dictionary<string, CodeTypeDeclaration> xsdTypeToSoType)
        {
			CodeTypeDeclaration soType;
			if (xsdTypeToSoType.TryGetValue(codeType.Name, out soType))
			{
				foreach (CodeTypeMember member in codeType.Members)
				{
					if (member is CodeMemberField)
					{
						var xsdMember = member as CodeMemberField;

						// TODO: This is a rather custom rework. Do we need this?
						var memberName = RemoveFieldNumberPartOfName(xsdMember.Name);
						var xsdMemberType = xsdMember.Type.BaseType;

						CodeMemberField soMember = null;
						CodeTypeDeclaration soMemberType;
						if (xsdTypeToSoType.TryGetValue(xsdMemberType, out soMemberType))
						{ // its type generated by this code generator

							soMember = new CodeMemberField(soMemberType.Name, memberName);

							if (xsdMember.Type.ArrayRank > 0)
							{ // its an array type, so we have to transfer this information also
								soMember.Type.ArrayRank = xsdMember.Type.ArrayRank;
								soMember.Type.ArrayElementType = new CodeTypeReference(soMemberType.Name);
							}
						}
						else
						{ // its some other type, e.g. a .Net primitive
							soMember = new CodeMemberField(xsdMember.Type, memberName);
						}

						soMember.Attributes = MemberAttributes.Public;
						soType.Members.Add(soMember);
					}
				}
			}
			else
			{
				Debug.LogError("Code generation error:\nThere is no serializable class type for the XSD type '" + codeType.Name + "'!");
			}

			return soType;
		}

		private string RemoveFieldNumberPartOfName(string name)
		{
			var fieldPartIdx = name.IndexOf("Field");
			return name.Substring(0, fieldPartIdx);
		}

		#endregion
	}
}