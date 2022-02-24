using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Archives {
    internal class ERFKeyListElement {
        internal AuroraResourceType resourceType;
        internal string resref;

        internal ERFKeyListElement(string resref, AuroraResourceType resourceType) {
            this.resref = resref;
            this.resourceType = resourceType;
        }

        public override bool Equals(object obj) {
            ERFKeyListElement otherElement = obj as ERFKeyListElement;
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
}
