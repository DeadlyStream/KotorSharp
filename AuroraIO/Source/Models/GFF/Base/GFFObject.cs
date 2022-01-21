using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AuroraIO.Source.Models.GFF.Helpers;
using AuroraIO.Source.Models.GFF;
using AuroraIO.Source.Common;
using System.Security;

namespace AuroraIO.Models {
    public class GFFObject : AuroraResource, ASCIIOutputProtocol {

        public Dictionary<String, GFFFieldDataObject> fields { get; private set; }

        public GFFObject(AuroraResourceType fileType) : this(fileType, new Dictionary<string, GFFFieldDataObject>()) {

        }

        public GFFObject (AuroraResourceType fileType, Dictionary<String, GFFFieldDataObject> fields) {
            this.fileType = fileType;
            this.fields = fields;
        }

        public GFFObject(AuroraResourceType fileType, GFFStruct structInfo) {
            this.fileType = fileType;
            this.fields = structInfo.fields;
        }

        public Object this[GFFPath path] {
            get {
                GFFStruct baseStruct = new GFFStruct(uint.MaxValue, fields);
                return baseStruct[path];
            } set {
                GFFStruct baseStruct = new GFFStruct(uint.MaxValue, fields);
                baseStruct[path] = value;
            }
        }

        public override byte[] toBytes () {
            return new GFFCoder().encode(this);
        }

        public override string ToString() {
            return String.Format("{{ {0} }}", new GFFStruct(uint.MaxValue, fields));
        }

        public string asciiEncoding() {
            return String.Format("{0}", new GFFStruct(uint.MaxValue, fields).asciiEncoding());
        }
    }
}