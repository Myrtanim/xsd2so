namespace Example.OptionalComplexTypes.Generated {
    
    
    [UnityEngine.CreateAssetMenuAttribute(fileName="rootTypeSO", menuName="Xsd2So/Create rootTypeSO", order=1)]
    public partial class rootTypeSO : UnityEngine.ScriptableObject {
        
        public versionData version;
        
        public animalDefinitions[] animals;
    }
    
    [System.SerializableAttribute()]
    public partial class versionData {
        
        public System.DateTime[] date;
        
        public int[] major;
        
        public int[] minor;
        
        public int[] fix;
    }
    
    [System.SerializableAttribute()]
    public partial class animalDefinitions {
        
        public animalType animal;
    }
    
    [System.SerializableAttribute()]
    public partial class animalType {
        
        public string name;
        
        public string description;
    }
}
