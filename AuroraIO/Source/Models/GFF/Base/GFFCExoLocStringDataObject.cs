using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFCExoLocStringDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.CEXOLOCSTRING;
        }

        public int subItemCount() {
            return 0;
        }

        public int strref;
        public Dictionary<GFFLanguage, string> cexoSubStrings { get; private set; }

        public GFFCExoLocStringDataObject(UInt32 strref, Dictionary<GFFLanguage, string> cexoSubStrings) {
            this.strref = (int)strref;
            this.cexoSubStrings = cexoSubStrings;
        }

        public void setValueForPath(object value, GFFPath path) {
            GFFPath firstPathComponent = path.first();
            GFFPath remainingPath = path.removingFirst();
            String lastPathComponent = path.last();

            if (firstPathComponent.Equals("strref")) {
                strref = Convert.ToInt32(value);
            } else {
                var gffLanguage = firstPathComponent.ToString().toGFFLanguage();
                var stringValue = value as String;
                cexoSubStrings[gffLanguage] = stringValue;
            }
        }

        public Object getValueAtPath(GFFPath path) {
            GFFPath firstPathComponent = path.first();
            GFFPath remainingPath = path.removingFirst();
            String lastPathComponent = path.last();

            if (firstPathComponent.Equals("strref")) {
                return strref;
            } else {
                return cexoSubStrings[firstPathComponent.ToString().toGFFLanguage()];
            }
        }

        public void setStringForLanguage(GFFLanguage language, string value) {
            cexoSubStrings[language] = value;
        }

        public int dataLength() {
            return 12
                + String.Join("", cexoSubStrings.Values).Length
                + cexoSubStrings.Keys.Count * 8;
        }

        public byte[] toBytes() {
            ByteArray cexoStringsArray = new ByteArray();
            foreach (KeyValuePair<GFFLanguage, string> tuple in cexoSubStrings) {
                cexoStringsArray.AddRange(BitConverter.GetBytes((UInt32)tuple.Key));
                cexoStringsArray.AddRange(BitConverter.GetBytes((UInt32)tuple.Value.Length));
                cexoStringsArray.AddRange(Encoding.ASCII.GetBytes(tuple.Value));
            }
            ByteArray newByteArray = new ByteArray();

            newByteArray.AddRange(BitConverter.GetBytes((UInt32)cexoStringsArray.Count + 8));
            newByteArray.AddRange(BitConverter.GetBytes((UInt32)strref));
            newByteArray.AddRange(BitConverter.GetBytes((UInt32)cexoSubStrings.Count));
            newByteArray.AddRange(cexoStringsArray.ToArray());

            return newByteArray.ToArray();
        }

        public override string ToString() {
            return String.Format("{{strref: {0}, substrings: [{1}]}}", strref, String.Join(",", cexoSubStrings.Select(pair => {
                return String.Format("{{{0}: {1}}}", pair.Key, pair.Value);
            }).ToArray()));
        }

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("value\nstrref {0}", strref));
            foreach (KeyValuePair<GFFLanguage, string> pair in cexoSubStrings) {
                sb.AppendLine(String.Format("language {0}", pair.Key));
                sb.AppendLine(String.Format("text '{0}'", pair.Value));
            }
            return sb.ToString();
        }
    }
}
