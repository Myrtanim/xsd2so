namespace Example.IncludedXsd.Generated {
    
    
    public enum buffHierarchyType {
        
        ALLIANCE,
        
        PLAYER,
        
        FARM,
        
        BUILDING_CATEGORY,
        
        BUILDING_GROUP,
        
        BUILDING,
    }
    
    [UnityEngine.CreateAssetMenuAttribute(fileName="buildingsTypeSO", menuName="Xsd2So/Create buildingsTypeSO", order=1)]
    public partial class buildingsTypeSO : UnityEngine.ScriptableObject {
        
        public buildingType[] building;
    }
    
    [System.SerializableAttribute()]
    public partial class buildingType {
        
        public int id;
        
        public bool idSpecified;
        
        public string name;
        
        public buffHierarchyType hierarchyType;
        
        public bool hierarchyTypeSpecified;
        
        public stageType[] stage;
    }
    
    [System.SerializableAttribute()]
    public partial class stageType {
        
        public int level;
        
        public bool levelSpecified;
        
        public bool buyable;
        
        public bool buyableSpecified;
        
        public string constrLimit;
        
        public int constrCost;
    }
    
    [System.SerializableAttribute()]
    public partial class buffType {
        
        public buffHierarchyType hierarchy;
        
        public int hierarchyId;
        
        public bool hierarchyIdSpecified;
        
        public int targetId;
        
        public bool targetIdSpecified;
        
        public double addition;
        
        public bool additionSpecified;
    }
}
