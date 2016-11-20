namespace Example.IncludedXsd.Generated.Editor {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true, ElementName="buildings")]
    public partial class buildingsType {
        
        private buildingType[] buildingField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("building")]
        public buildingType[] building {
            get {
                return this.buildingField;
            }
            set {
                this.buildingField = value;
            }
        }
        
        public void ToSerializable(Example.IncludedXsd.Generated.buildingsTypeSO buildingsTypeSO) {
            if ((this.building != null)) {
                buildingsTypeSO.building = new Example.IncludedXsd.Generated.buildingType[this.building.Length];
                for (int i = 0; (i < this.building.Length); i = (i + 1)) {
                    buildingsTypeSO.building[i] = new Example.IncludedXsd.Generated.buildingType();
                    this.building[i].ToSerializable(buildingsTypeSO.building[i]);
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class buildingType {
        
        private int idField;
        
        private bool idSpecifiedField;
        
        private string nameField;
        
        private buffHierarchyType hierarchyTypeField;
        
        private bool hierarchyTypeSpecifiedField;
        
        private stageType[] stageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public virtual bool idSpecified {
            get {
                return this.idSpecifiedField;
            }
            set {
                this.idSpecifiedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public buffHierarchyType hierarchyType {
            get {
                return this.hierarchyTypeField;
            }
            set {
                this.hierarchyTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public virtual bool hierarchyTypeSpecified {
            get {
                return this.hierarchyTypeSpecifiedField;
            }
            set {
                this.hierarchyTypeSpecifiedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("stage")]
        public stageType[] stage {
            get {
                return this.stageField;
            }
            set {
                this.stageField = value;
            }
        }
        
        public void ToSerializable(Example.IncludedXsd.Generated.buildingType buildingType) {
            buildingType.id = this.id;

            buildingType.idSpecified = this.idSpecified;

            buildingType.name = this.name;

            buildingType.hierarchyType = ((Example.IncludedXsd.Generated.buffHierarchyType)(this.hierarchyType));

            buildingType.hierarchyTypeSpecified = this.hierarchyTypeSpecified;

            if ((this.stage != null)) {
                buildingType.stage = new Example.IncludedXsd.Generated.stageType[this.stage.Length];
                for (int i = 0; (i < this.stage.Length); i = (i + 1)) {
                    buildingType.stage[i] = new Example.IncludedXsd.Generated.stageType();
                    this.stage[i].ToSerializable(buildingType.stage[i]);
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    public enum buffHierarchyType {
        
        /// <remarks/>
        ALLIANCE,
        
        /// <remarks/>
        PLAYER,
        
        /// <remarks/>
        FARM,
        
        /// <remarks/>
        BUILDING_CATEGORY,
        
        /// <remarks/>
        BUILDING_GROUP,
        
        /// <remarks/>
        BUILDING,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class stageType {
        
        private int levelField;
        
        private bool levelSpecifiedField;
        
        private bool buyableField;
        
        private bool buyableSpecifiedField;
        
        private string constrLimitField;
        
        private int constrCostField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int level {
            get {
                return this.levelField;
            }
            set {
                this.levelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public virtual bool levelSpecified {
            get {
                return this.levelSpecifiedField;
            }
            set {
                this.levelSpecifiedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool buyable {
            get {
                return this.buyableField;
            }
            set {
                this.buyableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public virtual bool buyableSpecified {
            get {
                return this.buyableSpecifiedField;
            }
            set {
                this.buyableSpecifiedField = value;
            }
        }
        
        /// <remarks/>
        public string constrLimit {
            get {
                return this.constrLimitField;
            }
            set {
                this.constrLimitField = value;
            }
        }
        
        /// <remarks/>
        public int constrCost {
            get {
                return this.constrCostField;
            }
            set {
                this.constrCostField = value;
            }
        }
        
        public void ToSerializable(Example.IncludedXsd.Generated.stageType stageType) {
            stageType.level = this.level;

            stageType.levelSpecified = this.levelSpecified;

            stageType.buyable = this.buyable;

            stageType.buyableSpecified = this.buyableSpecified;

            stageType.constrLimit = this.constrLimit;

            stageType.constrCost = this.constrCost;

        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class buffType {
        
        private buffHierarchyType hierarchyField;
        
        private int hierarchyIdField;
        
        private bool hierarchyIdSpecifiedField;
        
        private int targetIdField;
        
        private bool targetIdSpecifiedField;
        
        private double additionField;
        
        private bool additionSpecifiedField;
        
        /// <remarks/>
        [System.ComponentModel.DefaultValue(buffHierarchyType.PLAYER)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public buffHierarchyType hierarchy {
            get {
                return this.hierarchyField;
            }
            set {
                this.hierarchyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int hierarchyId {
            get {
                return this.hierarchyIdField;
            }
            set {
                this.hierarchyIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public virtual bool hierarchyIdSpecified {
            get {
                return this.hierarchyIdSpecifiedField;
            }
            set {
                this.hierarchyIdSpecifiedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int targetId {
            get {
                return this.targetIdField;
            }
            set {
                this.targetIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public virtual bool targetIdSpecified {
            get {
                return this.targetIdSpecifiedField;
            }
            set {
                this.targetIdSpecifiedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double addition {
            get {
                return this.additionField;
            }
            set {
                this.additionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public virtual bool additionSpecified {
            get {
                return this.additionSpecifiedField;
            }
            set {
                this.additionSpecifiedField = value;
            }
        }
        
        public void ToSerializable(Example.IncludedXsd.Generated.buffType buffType) {
            buffType.hierarchy = ((Example.IncludedXsd.Generated.buffHierarchyType)(this.hierarchy));

            buffType.hierarchyId = this.hierarchyId;

            buffType.hierarchyIdSpecified = this.hierarchyIdSpecified;

            buffType.targetId = this.targetId;

            buffType.targetIdSpecified = this.targetIdSpecified;

            buffType.addition = this.addition;

            buffType.additionSpecified = this.additionSpecified;

        }
    }
}
