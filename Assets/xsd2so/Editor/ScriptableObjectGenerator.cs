using System;
using System.CodeDom;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Xsd2So
{
    internal class ScriptableObjectGenerator
    {
		private const string SERIALIZABLE_METHOD_NAME = "ToSerializable";

		private string soClassPostfix;

		public ScriptableObjectGenerator(string soClassPostfix)
		{
			this.soClassPostfix = soClassPostfix;
		}

		public void Execute(GenerationContext ctx)
		{
			Dictionary<string, CodeTypeDeclaration> xsdTypeNameToSoTypeDecl = new Dictionary<string, CodeTypeDeclaration>();

			// Step 1:
			// Generate all SO types without their members, so that we have all required types available.
			foreach (DataRepresentation xsdType in ctx.XsdCodeMapping)
			{
				var copiedType = CreateSoType(xsdType.CodeType, ctx);
				xsdTypeNameToSoTypeDecl.Add(xsdType.CodeType.Name, copiedType);

				if (copiedType.IsEnum)
				{
					ctx.ScriptableObjectCode.Types.Add(copiedType);
				}
			}

			// Step 2:
			// Generate members of SO types and replace XSD member types with their SO equivalents.
            foreach (DataRepresentation xsdType in ctx.XsdCodeMapping)
            {
				if (!xsdType.CodeType.IsEnum)
				{
					CodeTypeDeclaration soType = CopyMembers(xsdType.CodeType, xsdTypeNameToSoTypeDecl);
					CreateCopyMethod(soType, xsdType.CodeType, xsdTypeNameToSoTypeDecl, ctx);

					ctx.ScriptableObjectCode.Types.Add(soType);
				}
			}
		}

		#region Code generation for data copy method XSD -> SO

		private void CreateCopyMethod(
			CodeTypeDeclaration soType,
			CodeTypeDeclaration xsdType,
			Dictionary<string, CodeTypeDeclaration> xsdTypeNameToSoTypeDecl,
			GenerationContext ctx
		)
		{
			// create ToSerializable method for this xsdType
			var toSerializableMethod = new CodeMemberMethod();
			toSerializableMethod.Name = SERIALIZABLE_METHOD_NAME;
			toSerializableMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			toSerializableMethod.ReturnType = new CodeTypeReference(typeof(void));

			var paramType = ctx.ScriptableObjectCode.Name + "." + soType.Name;
			var paramName = soType.Name.ToFirstLetterLowerCase();
			var parameter = new CodeParameterDeclarationExpression(paramType, paramName);
			toSerializableMethod.Parameters.Add(parameter);

			foreach (CodeTypeMember xsdMember in xsdType.Members)
			{
				if (xsdMember is CodeMemberProperty)
				{
					var xsdProperty = xsdMember as CodeMemberProperty;
					var xsdPropertyType = xsdProperty.Type.BaseType;

					bool isXsdType = xsdTypeNameToSoTypeDecl.ContainsKey(xsdPropertyType);

					CodeStatement transferStatement = null;
					if (isXsdType)
					{
						var xsdMemberType = ctx.XsdCodeMapping.First(ele => ele.CodeType.Name == xsdPropertyType);
						if (xsdMemberType == null)
						{
							Debug.LogError("No Xsd Code mapping found for '" + xsdPropertyType + "'");
							continue;
						}

						var xsdMemberCodeDeclaration = xsdMemberType.CodeType;
						var targetSoTypeFQN = ctx.ScriptableObjectCode.Name + "." + xsdTypeNameToSoTypeDecl[xsdMemberCodeDeclaration.Name].Name;
						if (xsdMemberCodeDeclaration.IsEnum) // handle enum members with Xsd types
						{
							transferStatement = new CodeAssignStatement(
								new CodePropertyReferenceExpression(
									new CodeVariableReferenceExpression(paramName),
									xsdProperty.Name.ToFirstLetterLowerCase()
								),
								new CodeCastExpression(
									new CodeTypeReference(targetSoTypeFQN),
									new CodePropertyReferenceExpression(
										new CodeThisReferenceExpression(),
										xsdProperty.Name
									)
								)
							);
						}
						else
						{
							if (xsdProperty.Type.ArrayRank == 0) // handle normal members with Xsd types
							{
								// first, create a new object of the SO field, to prevent NPE
								toSerializableMethod.Statements.Add(new CodeAssignStatement(
										new CodePropertyReferenceExpression(
											new CodeVariableReferenceExpression(paramName),
											xsdProperty.Name.ToFirstLetterLowerCase()
										),
										new CodeObjectCreateExpression(targetSoTypeFQN)
									)
								);

								// now call recursivly ToSerializable to further transfer the data
								transferStatement = new CodeExpressionStatement(
									new CodeMethodInvokeExpression(
										new CodePropertyReferenceExpression(
											new CodeThisReferenceExpression(),
											xsdProperty.Name
										),
										SERIALIZABLE_METHOD_NAME,
										new CodeExpression[] {
											new CodeFieldReferenceExpression(
												new CodeArgumentReferenceExpression(paramName),
												xsdProperty.Name.ToFirstLetterLowerCase()
											)
										}
									)
								);
							}
							else // handle array members with Xsd types (that's the fun part! :) )
							{ // we want to generate this code:
							  //		1   if (this.xsdArrayProperty != null)
							  //		2   {
							  //		3		soObject.soField = new soType[this.xsdArrayProperty.Length];
							  //		4		for (int i = 0; i < this.xsdArrayProperty.Length;  i = i + 1)
							  //		5		{
							  //		6			soObject.soField[i] = new soType();
							  //		7			this.xsdArrayProperty[i].ToSerializable(soObject.soField[i]);
							  //		8		}
							  //		9	}

								// 1-3: null check & array creation
								toSerializableMethod.Statements.Add(
									new CodeConditionStatement( // if (this.xsdArrayProperty != null) ...
										new CodeBinaryOperatorExpression( // ... this.xsdArrayProperty != null ...
											new CodePropertyReferenceExpression( // ... this.xsdArrayProperty ...
													new CodeThisReferenceExpression(),
													xsdProperty.Name),
											CodeBinaryOperatorType.IdentityInequality, // ... != ...
											new CodeSnippetExpression("null") // ... null)
										),
										new CodeAssignStatement( // ... = ... 
											new CodePropertyReferenceExpression( // soObject.soField ...
												new CodeVariableReferenceExpression(paramName),
												xsdProperty.Name.ToFirstLetterLowerCase()
											),
											new CodeArrayCreateExpression(
												targetSoTypeFQN,
												new CodePropertyReferenceExpression( // ... this.xsdArrayProperty.Length ...
													new CodePropertyReferenceExpression( // ... this.xsdArrayProperty ...
														new CodeThisReferenceExpression(),
														xsdProperty.Name),
													"Length"
												)
											)
										),
										// 4-8: iterate over array, create object, transfer data
										new CodeIterationStatement( // for ... { ... }
											new CodeVariableDeclarationStatement( // ... (i = 0; ...
												typeof(int),
												"i",
												new CodePrimitiveExpression(0)
											),
											new CodeBinaryOperatorExpression( // ...; i < this.xsdArrayProperty.Length; ...
												new CodeVariableReferenceExpression("i"),
												CodeBinaryOperatorType.LessThan,
												new CodePropertyReferenceExpression( // ... this.xsdArrayProperty.Length ...
													new CodePropertyReferenceExpression( // ... this.xsdArrayProperty ...
														new CodeThisReferenceExpression(),
														xsdProperty.Name),
													"Length"
												)
											),
											new CodeAssignStatement( // ... ; i = i + 1) ... // wo don't have the unary increment operator in CodeDom
												new CodeVariableReferenceExpression("i"),
												new CodeBinaryOperatorExpression(
													new CodeVariableReferenceExpression("i"),
													CodeBinaryOperatorType.Add,
													new CodePrimitiveExpression(1)
												)
											),
											new CodeStatement[] { // ... { ... }
												new CodeAssignStatement( // soObject.soField[i] = new soType();
													new CodeArrayIndexerExpression( // soObject.soField[i] ...
														new CodePropertyReferenceExpression(
															new CodeVariableReferenceExpression(paramName),
															xsdProperty.Name.ToFirstLetterLowerCase()
														),
														new CodeVariableReferenceExpression("i")
													),
													new CodeObjectCreateExpression(targetSoTypeFQN)
												),
												new CodeExpressionStatement( // this.xsdArrayProperty[i].ToSerializable(soObject.soField[i]);
													new CodeMethodInvokeExpression(
														new CodeArrayIndexerExpression(
															new CodePropertyReferenceExpression(
																new CodeThisReferenceExpression(),
																xsdProperty.Name
															),
															new CodeVariableReferenceExpression("i")
														),
														SERIALIZABLE_METHOD_NAME,
														new CodeExpression[] {
															new CodeArrayIndexerExpression(
																new CodeFieldReferenceExpression(
																	new CodeArgumentReferenceExpression(paramName),
																	xsdProperty.Name.ToFirstLetterLowerCase()
																),
																new CodeVariableReferenceExpression("i")
															)
														}
													)
												)
											}
										)
									)
								);
							}
						}
					}
					else
					{
						// TODO need handle array types here, too!

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
						toSerializableMethod.Statements.Add(new CodeSnippetStatement());
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
				if (codeType.Name == context.Config.XsdRootElementTypeName)
				{
					return CreateSoRootClass(codeType, context);
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

		private CodeTypeDeclaration CreateSoRootClass(CodeTypeDeclaration xsdType, GenerationContext context)
		{
			foreach (CodeAttributeDeclaration attribute in xsdType.CustomAttributes)
			{
				if (attribute.Name == "System.Xml.Serialization.XmlRootAttribute")
				{
					bool rootNameFound = false;
					foreach (CodeAttributeArgument attriArg in attribute.Arguments)
					{
						if (attriArg.Value is CodePrimitiveExpression)
						{
							var expr = attriArg.Value as CodePrimitiveExpression;
							if (expr.Value is string && (string)expr.Value == context.Config.XmlRootNodeName)
							{
								rootNameFound = true;
								break;
							}
						}
					}

					if (!rootNameFound)
					{
						attribute.Arguments.Add(
							new CodeAttributeArgument(
								new CodeSnippetExpression("ElementName=\"" + context.Config.XmlRootNodeName + "\"")
							)
						);
					}
				}
			}

			var rootClass = CreateSoClass(xsdType);
			rootClass.Name += soClassPostfix;
			rootClass.CustomAttributes.Clear();
			rootClass.BaseTypes.Add(new CodeTypeReference(typeof(ScriptableObject)));

			// Create a shortcut in Unity's Creat menu, to be able to create the new SO type from UI.
			CodeDomHelper.AddAttribute(rootClass,
				typeof(CreateAssetMenuAttribute),
				new Pair<string, object>("fileName", rootClass.Name),
				new Pair<string, object>("menuName", "Xsd2So/Create " + rootClass.Name),
				new Pair<string, object>("order", 1)
			);

			return rootClass;
		}

		private CodeTypeDeclaration CreateSoClass(CodeTypeDeclaration xsdType)
		{
			var r = new CodeTypeDeclaration(xsdType.Name);
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

		private CodeTypeDeclaration CopyMembers(CodeTypeDeclaration xsdType, Dictionary<string, CodeTypeDeclaration> xsdTypeToSoType)
        {
			CodeTypeDeclaration soType;
			if (xsdTypeToSoType.TryGetValue(xsdType.Name, out soType))
			{
				foreach (CodeTypeMember member in xsdType.Members)
				{
					if (member is CodeMemberField)
					{
						var xsdMember = member as CodeMemberField;

						// TODO: This is a rather custom rework. Do we need this?

						// Removes the number of XSD fields, e.g. "idField0" becomes "idField".
						// The XmlSchema compiler adds the numbers to prevent name collision, because he
						// sees the whole schema as one "block". I suppose.
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
				Debug.LogError("Code generation error:\nThere is no serializable class type for the XSD type '" + xsdType.Name + "'!");
			}

			return soType;
		}

		private string RemoveFieldNumberPartOfName(string name)
		{
			var fieldPartIdx = name.IndexOf("Field", StringComparison.InvariantCulture);
			return name.Substring(0, fieldPartIdx);
		}

		#endregion
	}
}