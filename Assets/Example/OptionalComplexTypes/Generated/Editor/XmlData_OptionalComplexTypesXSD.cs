namespace Example.OptionalComplexTypes.Generated.Editor {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.myrtanim.de", IsNullable=true, ElementName="root")]
    public partial class rootType {
        
        private versionData versionField;
        
        private animalDefinitions[] animalsField;
        
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
        [System.Xml.Serialization.XmlElementAttribute("animals")]
        public animalDefinitions[] animals {
            get {
                return this.animalsField;
            }
            set {
                this.animalsField = value;
            }
        }
        
        public void ToSerializable(Example.OptionalComplexTypes.Generated.rootTypeSO rootTypeSO) {
            rootTypeSO.version = new Example.OptionalComplexTypes.Generated.versionData();
            this.version.ToSerializable(rootTypeSO.version);

            if ((this.animals != null)) {
                rootTypeSO.animals = new Example.OptionalComplexTypes.Generated.animalDefinitions[this.animals.Length];
                for (int i = 0; (i < this.animals.Length); i = (i + 1)) {
                    rootTypeSO.animals[i] = new Example.OptionalComplexTypes.Generated.animalDefinitions();
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
        
        private System.DateTime[] dateField;
        
        private int[] majorField;
        
        private int[] minorField;
        
        private int[] fixField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("date")]
        public System.DateTime[] date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("major")]
        public int[] major {
            get {
                return this.majorField;
            }
            set {
                this.majorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("minor")]
        public int[] minor {
            get {
                return this.minorField;
            }
            set {
                this.minorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("fix")]
        public int[] fix {
            get {
                return this.fixField;
            }
            set {
                this.fixField = value;
            }
        }
        
        public void ToSerializable(Example.OptionalComplexTypes.Generated.versionData versionData) {
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
    public partial class animalDefinitions {
        
        private animalType animalField;
        
        /// <remarks/>
        public animalType animal {
            get {
                return this.animalField;
            }
            set {
                this.animalField = value;
            }
        }
        
        public void ToSerializable(Example.OptionalComplexTypes.Generated.animalDefinitions animalDefinitions) {
            animalDefinitions.animal = new Example.OptionalComplexTypes.Generated.animalType();
            this.animal.ToSerializable(animalDefinitions.animal);

        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.myrtanim.de")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class animalType {
        
        private string nameField;
        
        private string descriptionField;
        
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
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        public void ToSerializable(Example.OptionalComplexTypes.Generated.animalType animalType) {
            animalType.name = this.name;

            animalType.description = this.description;

        }
    }
}
