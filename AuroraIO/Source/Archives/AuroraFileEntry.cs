using AuroraIO.Models.Base;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Source.Archives {
    public class AuroraResourceName {
        public CResRef resref;
        public AuroraResourceType resourceType;

        public AuroraResourceName(string resref, AuroraResourceID resourceID) {
            this.resref = resref;
            this.resourceType = (int)resourceID;
        }

        public AuroraResourceName(string resref, string resourceType) {
            this.resref = resref;
            this.resourceType = resourceType;
        }

        public override bool Equals(object obj) {
            AuroraResourceName otherElement = obj as AuroraResourceName;
            return otherElement.resref == resref
                && otherElement.resourceType.id == resourceType.id;
        }

        public override int GetHashCode() {
            return resref.GetHashCode() + resourceType.GetHashCode();
        }

        public static implicit operator AuroraResourceName(string value) {
            string resref = Regex.Match(value, "^[^.]*").Value.ToLower();
            string resourceType = Regex.Match(value, "[^.]*$").Value;
            return new AuroraResourceName(resref, resourceType);
        }

        public static implicit operator String(AuroraResourceName name) {
            return name.ToString();
        }

        public override string ToString() {
            return String.Format("{0}.{1}", resref, resourceType.stringValue);
        }
    }

    public class AuroraFileEntry: ASCIIEncodingProtocol {
        public AuroraResourceName name;
        public byte[] data;

        public AuroraFileEntry(string name, byte[] data) {
            this.name = name;
            this.data = data;
        }

        public AuroraFileEntry(AuroraResourceName name, byte[] data) {
            this.name = name;
            this.data = data;
        }

        public string asciiEncoding(string indent = "") {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}-\n", indent);
            sb.AppendFormat("{0}  name: {1}\n", indent, name);
            sb.AppendFormat("{0}  size: {1}\n", indent, data.Length);
            return sb.ToString();
        }
    }
}
