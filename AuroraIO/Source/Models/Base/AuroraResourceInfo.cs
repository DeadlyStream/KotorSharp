using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO {
    public class AuroraResourceInfo {
        public AuroraResourceType resourceType;
        public string resref;

        public AuroraResourceInfo(string resref, AuroraResourceType resourceType) {
            this.resref = resref;
            this.resourceType = resourceType;
        }

        public override bool Equals(object obj) {
            AuroraResourceInfo otherElement = obj as AuroraResourceInfo;
            return otherElement.resref == resref
                && otherElement.resourceType == resourceType;
        }

        public override int GetHashCode() {
            return resref.GetHashCode() + resourceType.GetHashCode();
        }

        public override string ToString() {
            return resref + "." + resourceType.stringValue();
        }
    }

    public static class AuroraResourceInfoExtensions {
        public static AuroraResourceInfo toAuroraResourceInfo(this string s) {
            String fullFileName = Regex.Match(s, "[^\\\\]*$").Value.ToLower();
            string resref = Regex.Match(fullFileName, "^[^.]*").Value.ToLower();
            AuroraResourceType resourceType = Regex.Match(fullFileName, "[^.]*$").Value.toAuroraResourceType();
            return new AuroraResourceInfo(resref, resourceType);
        }
    }
}
