using System.CodeDom;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Xsd2So
{
    public class DataRepresentation
    {
        public XmlSchemaAnnotated XsdType { get; set; }
        public XmlTypeMapping TypeMapping { get; set; }
        public CodeTypeDeclaration CodeType { get; set; }

        public bool IsArray { get; internal set; }

        public bool IsComplexXsdType
        {
            get { return XsdType is XmlSchemaComplexType; }
        }
    }

//	public class DataTypeRepresentation : DataRepresentation
//	{
//		public XmlSchemaType XsdType { get; set; }
//	}
//
//	public class DataElementRepresentation : DataRepresentation
//	{
//		public XmlSchemaElement XsdType { get; set; }
//	}

    class ModifiableRepresentationDuplicateComparer : IEqualityComparer<DataRepresentation>
    {
        public bool Equals(DataRepresentation x, DataRepresentation y)
        {
            return x.TypeMapping.ElementName == y.TypeMapping.ElementName;
        }

        public int GetHashCode(DataRepresentation obj)
        {
            return obj.TypeMapping.ElementName.GetHashCode();
        }
    }
}