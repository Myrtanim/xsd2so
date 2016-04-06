using System;
using System.CodeDom;

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
                var duplicatedType = Copy(xsdType.CodeType);
                CreateCopyMethod(duplicatedType);
                
                ctx.ScriptableObjectCode.Types.Add(duplicatedType);
            }
        }

        private void CreateCopyMethod(CodeTypeDeclaration duplicatedType)
        {
            
        }

        private CodeTypeDeclaration Copy(CodeTypeDeclaration codeType)
        {
            if (codeType.IsClass)
            {
                return CopyClass(codeType);
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
            r.IsClass = true;

            foreach (CodeTypeMember member in codeType.Members)
            {
                if (member is CodeMemberField)
                {
                    var m = member as CodeMemberField;

                    var mf = new CodeMemberField(m.Type, m.Name);
                    mf.Attributes = MemberAttributes.Public;

                    // replace xxxSpecified with nullable type
                    // rename property and cut xxxField out

                    r.Members.Add(mf);
                }
            }

            return r;
        }
    }
}