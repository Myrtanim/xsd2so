namespace Example.VenetianBlindTest2.Generated.Editor {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.myrtanim.de", IsNullable=true)]
    public enum plantTypes {
        
        /// <remarks/>
        roots,
        
        /// <remarks/>
        trees,
        
        /// <remarks/>
        herbs,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.myrtanim.de", IsNullable=true)]
    public enum sizeCategory {
        
        /// <remarks/>
        fine,
        
        /// <remarks/>
        diminutive,
        
        /// <remarks/>
        tiny,
        
        /// <remarks/>
        small,
        
        /// <remarks/>
        medium,
        
        /// <remarks/>
        large,
        
        /// <remarks/>
        huge,
        
        /// <remarks/>
        gargantuan,
        
        /// <remarks/>
        colossal,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.myrtanim.de", IsNullable=true, ElementName="root")]
    public partial class rootType {
        
        private versionData versionField;
        
        private baseAnimal[] animalsField;
        
        /// <remarks/>
        public versionData version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem(ElementName="herbivor", Type=typeof(herbivorType), IsNullable=false)]
        [System.Xml.Serialization.XmlArrayItem(ElementName="carnivor", Type=typeof(carnivorType), IsNullable=false)]
        public baseAnimal[] animals {
            get {
                return this.animalsField;
            }
            set {
                this.animalsField = value;
            }
        }
        
        public void ToSerializable(Example.VenetianBlindTest2.Generated.rootTypeSO rootTypeSO) {
            rootTypeSO.version = new Example.VenetianBlindTest2.Generated.versionData();
            this.version.ToSerializable(rootTypeSO.version);

            if ((this.animals != null)) {
                rootTypeSO.animals = new Example.VenetianBlindTest2.Generated.baseAnimal[this.animals.Length];
                for (int i = 0; (i < this.animals.Length); i = (i + 1)) {
                    rootTypeSO.animals[i] = new Example.VenetianBlindTest2.Generated.baseAnimal();
                    this.animals[i].ToSerializable(rootTypeSO.animals[i]);
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class versionData {
        
        private System.DateTime dateField;
        
        private int majorField;
        
        private int minorField;
        
        private int fixField;
        
        /// <remarks/>
        public System.DateTime date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        public int major {
            get {
                return this.majorField;
            }
            set {
                this.majorField = value;
            }
        }
        
        /// <remarks/>
        public int minor {
            get {
                return this.minorField;
            }
            set {
                this.minorField = value;
            }
        }
        
        /// <remarks/>
        public int fix {
            get {
                return this.fixField;
            }
            set {
                this.fixField = value;
            }
        }
        
        public void ToSerializable(Example.VenetianBlindTest2.Generated.versionData versionData) {
            versionData.date = this.date;

            versionData.major = this.major;

            versionData.minor = this.minor;

            versionData.fix = this.fix;

        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class herbivorType : baseAnimal {
        
        private plantTypes favoritePlantTypeField;
        
        /// <remarks/>
        [System.ComponentModel.DefaultValue(plantTypes.herbs)]
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public plantTypes favoritePlantType {
            get {
                return this.favoritePlantTypeField;
            }
            set {
                this.favoritePlantTypeField = value;
            }
        }
        
        public void ToSerializable(Example.VenetianBlindTest2.Generated.herbivorType herbivorType) {
            herbivorType.favoritePlantType = ((Example.VenetianBlindTest2.Generated.plantTypes)(this.favoritePlantType));

        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(carnivorType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(herbivorType))]
    public partial class baseAnimal {
        
        private string nameField;
        
        private sizeCategory sizeField;
        
        private bool sizeSpecifiedField;
        
        private string descriptionField;
        
        private attributeLevel[] attributeLevelsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public sizeCategory size {
            get {
                return this.sizeField;
            }
            set {
                this.sizeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public virtual bool sizeSpecified {
            get {
                return this.sizeSpecifiedField;
            }
            set {
                this.sizeSpecifiedField = value;
            }
        }
        
        /// <remarks/>
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem(ElementName="level", IsNullable=false)]
        public attributeLevel[] attributeLevels {
            get {
                return this.attributeLevelsField;
            }
            set {
                this.attributeLevelsField = value;
            }
        }
        
        public void ToSerializable(Example.VenetianBlindTest2.Generated.baseAnimal baseAnimal) {
            baseAnimal.name = this.name;

            baseAnimal.size = ((Example.VenetianBlindTest2.Generated.sizeCategory)(this.size));

            baseAnimal.sizeSpecified = this.sizeSpecified;

            baseAnimal.description = this.description;

            if ((this.attributeLevels != null)) {
                baseAnimal.attributeLevels = new Example.VenetianBlindTest2.Generated.attributeLevel[this.attributeLevels.Length];
                for (int i = 0; (i < this.attributeLevels.Length); i = (i + 1)) {
                    baseAnimal.attributeLevels[i] = new Example.VenetianBlindTest2.Generated.attributeLevel();
                    this.attributeLevels[i].ToSerializable(baseAnimal.attributeLevels[i]);
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class attributeLevel {
        
        private int levelField;
        
        private sbyte strengthField;
        
        private sbyte speedField;
        
        private int hitPointsField;
        
        private bool canFlyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public int level {
            get {
                return this.levelField;
            }
            set {
                this.levelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public sbyte strength {
            get {
                return this.strengthField;
            }
            set {
                this.strengthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public sbyte speed {
            get {
                return this.speedField;
            }
            set {
                this.speedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public int hitPoints {
            get {
                return this.hitPointsField;
            }
            set {
                this.hitPointsField = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValue(false)]
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public bool canFly {
            get {
                return this.canFlyField;
            }
            set {
                this.canFlyField = value;
            }
        }
        
        public void ToSerializable(Example.VenetianBlindTest2.Generated.attributeLevel attributeLevel) {
            attributeLevel.level = this.level;

            attributeLevel.strength = this.strength;

            attributeLevel.speed = this.speed;

            attributeLevel.hitPoints = this.hitPoints;

            attributeLevel.canFly = this.canFly;

        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class carnivorType : baseAnimal {
        
        private string favoriteHerbivorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Namespace="")]
        public string favoriteHerbivor {
            get {
                return this.favoriteHerbivorField;
            }
            set {
                this.favoriteHerbivorField = value;
            }
        }
        
        public void ToSerializable(Example.VenetianBlindTest2.Generated.carnivorType carnivorType) {
            carnivorType.favoriteHerbivor = this.favoriteHerbivor;

        }
    }
}
