using System;
using System.CodeDom;
using UnityEngine;
using UnityEditor;

namespace Xsd2So
{
    internal class ScriptableObjectGenerator : ICodeModifier
    {
        public void Execute(GenerationContext ctx)
        {
            // 1. create a new namespace, which is accessible for non-editor code
            // 2. for all Xsd types
            //    a) generate serializable type
            //    b) add serializable type to new namespace
            //    c) generate content-transfering method to copy Xsd type data to SO type data (e.g. "XsdType.Copy(soType)"

            foreach (var xsdType in ctx.XsdCodeMapping)
            {
                var duplicatedType = Copy(xsdType.CodeType, ctx);
                CreateCopyMethod(duplicatedType, xsdType.CodeType);
                
                ctx.ScriptableObjectCode.Types.Add(duplicatedType);
            }
        }

        private void CreateCopyMethod(CodeTypeDeclaration serializableType, CodeTypeDeclaration xsdType)
        {
			// need special handling for root element

			// create static helper class/method? or just embed the ToSerializable() method?
			// Problem is, we can't new ScriptableObjects.
			// But we can do
			//		ScriptableObject.CreateInstance<RootClassType>()
			// and then in the recursivly do the ToSerializable()...

			// TODO: write a example how the code should look like, beginning from the
		}

		private CodeTypeDeclaration Copy(CodeTypeDeclaration codeType, GenerationContext context)
        {
            if (codeType.IsClass)
            {
				if (codeType.Name == context.RootElementTypeName)
				{
					return CopyRootClass(codeType);
				}
				else
				{
					return CopyClass(codeType);
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

		private CodeTypeDeclaration CopyRootClass(CodeTypeDeclaration codeType)
		{
			var rootClass = CopyClass(codeType);
			rootClass.CustomAttributes.Clear();
			rootClass.BaseTypes.Add(new CodeTypeReference(typeof(ScriptableObject)));

			// TODO: this is optional and does not necessarily have to be, since the
			// xsdType.ToSerializable() exists...
			CodeDomHelper.AddAttribute(rootClass,
				typeof(CreateAssetMenuAttribute),
				new Pair<string, object>("fileName", codeType.Name),
				new Pair<string, object>("menuName", "Xsd2So/Create " + codeType.Name),
				new Pair<string, object>("order", 1)
			);

			return rootClass;
		}

		private CodeTypeDeclaration CopyEnum(CodeTypeDeclaration codeType)
        {
            var r = new CodeTypeDeclaration(codeType.Name);
            r.IsEnum = true;

            foreach(CodeMemberField member in codeType.Members)
            {
                var enumVal = new CodeMemberField(member.Type, member.Name);
                r.Members.Add(enumVal);
            }

            return r;
        }

        private CodeTypeDeclaration CopyClass(CodeTypeDeclaration codeType)
        {
            var r = new CodeTypeDeclaration(codeType.Name);
            r.IsClass = true; // make it a class
			r.IsPartial = true; // make it a partial class

			// Add Serializable attribute
			CodeDomHelper.AddAttribute(r, typeof(SerializableAttribute));
			
            foreach (CodeTypeMember member in codeType.Members)
            {
                if (member is CodeMemberField)
                {
                    var m = member as CodeMemberField;

					// TODO: This is a rather custom rework. Do we need this?
					var memberName = RemoveFieldNumberPartOfName(m.Name);

                    var mf = new CodeMemberField(m.Type, memberName);
                    mf.Attributes = MemberAttributes.Public;
					
					// Maybe add a getter which is baked by the generated field?
					// then also the field itself can be private, with a SerializeField attribute,
					// and thus make it readonly. Which is a good idea, since this is static data,
					// that should not be altered.

                    r.Members.Add(mf);
                }
            }

            return r;
        }

		private string RemoveFieldNumberPartOfName(string name)
		{
			var fieldPartIdx = name.IndexOf("Field");
			return name.Substring(0, fieldPartIdx);
		}
	}
}