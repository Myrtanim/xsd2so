namespace Example.VenetianBlindTest2.Generated {
    
    
    public enum plantTypes {
        
        roots,
        
        trees,
        
        herbs,
    }
    
    public enum sizeCategory {
        
        fine,
        
        diminutive,
        
        tiny,
        
        small,
        
        medium,
        
        large,
        
        huge,
        
        gargantuan,
        
        colossal,
    }
    
    [UnityEngine.CreateAssetMenuAttribute(fileName="rootTypeSO", menuName="Xsd2So/Create rootTypeSO", order=1)]
    public partial class rootTypeSO : UnityEngine.ScriptableObject {
        
        public versionData version;
        
        public baseAnimal[] animals;
    }
    
    [System.SerializableAttribute()]
    public partial class versionData {
        
        public System.DateTime date;
        
        public int major;
        
        public int minor;
        
        public int fix;
    }
    
    [System.SerializableAttribute()]
    public partial class baseAnimal {
        
        public string name;
        
        public sizeCategory size;
        
        public bool sizeSpecified;
        
        public string description;
        
        public attributeLevel[] attributeLevels;
    }
    
    [System.SerializableAttribute()]
    public partial class herbivorType {
        
        public plantTypes favoritePlantType;
    }
    
    [System.SerializableAttribute()]
    public partial class carnivorType {
        
        public string favoriteHerbivor;
    }
    
    [System.SerializableAttribute()]
    public partial class attributeLevel {
        
        public int level;
        
        public sbyte strength;
        
        public sbyte speed;
        
        public int hitPoints;
        
        public bool canFly;
    }
}
