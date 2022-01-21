using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.GFF.Base {
    public class GFFPathComponent {
        private string internalComponent;

        GFFPathComponent(String internalComponent) {
            this.internalComponent = internalComponent;
        }

        public static implicit operator GFFPathComponent(string address) {
            return new GFFPathComponent(address);
        }

        public static implicit operator String(GFFPathComponent path) {
            return path.internalComponent;
        }

        public static string operator +(String originalString, GFFPathComponent path) {
            return originalString + "\\" + path.internalComponent;
        }
    }
}
