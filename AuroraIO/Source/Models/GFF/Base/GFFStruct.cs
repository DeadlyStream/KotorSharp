using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.GFF;
using AuroraIO.Source.Models.GFF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFStruct: ASCIIOutputProtocol {
        public uint structType;
        public Dictionary<String, GFFFieldDataObject> fields;

        public GFFStruct(UInt32 value) : this(value, new Dictionary<string, GFFFieldDataObject>()) {
            this.structType = value;
        }

        public GFFStruct(UInt32 value, Dictionary<String, GFFFieldDataObject> fields) {
            this.structType = value;
            this.fields = fields;
        }

        public GFFStruct(UInt32 value, KeyValuePair<String, GFFFieldDataObject> fieldInfo) {
            structType = value;
            fields = new Dictionary<string, GFFFieldDataObject>();
            fields[fieldInfo.Key] = fieldInfo.Value;
        }

        public Object this[GFFPath path] {
            get {
                return getObject(path);
            } set {
                setObjectAtPath(value, path);
            }
        }

        private void setObjectAtPath(object value, GFFPath path) {
            String fieldName = path.first();
            GFFPath nextPath = path.removingFirst();
            if (fields.ContainsKey(fieldName)) {
                fields[fieldName].setValueForPath(value, nextPath);
            } else if (value is GFFFieldDataObject) {
                var dataObject = value as GFFFieldDataObject;
                fields[fieldName] = dataObject;
            }
        }

        private Object getObject(GFFPath path) {
            String fieldName = path.first();

            if (fields.ContainsKey(fieldName)) {
                GFFPath subPath = path.removingFirst();
                return fields[fieldName].getValueAtPath(subPath);
            } else {
                return null;
            }
        }

        public override string ToString() {
            return String.Format("id: {0}, fields: {{{1}}}", structType, String.Join(",", fields.Select(field => {
                return String.Format("{0}: {{{1}}}", field.Key, field.Value.ToString());
            })));
        }

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("struct"));
            sb.AppendLine(String.Format("id {0}", structType));
            
            foreach (KeyValuePair<String, GFFFieldDataObject> pair in fields) {
                sb.AppendLine(String.Format("beginfield"));
                sb.AppendLine(String.Format("type {0}", pair.Value.fieldType().stringValue()));
                sb.AppendLine(String.Format("label {0}", pair.Key));
                sb.AppendLine(String.Format("{0}", pair.Value.asciiEncoding("")));
                sb.AppendLine(String.Format("endfield"));
            }
            
            return sb.ToString();
        }
    }
}
