using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Base {
    public struct CResRef: IComparable {

        public int Length => value.Length;

        private string value;
        private CResRef(string value) {
            this.value = value.TrimEnd('\0').Truncate(16);
        }

        public static implicit operator CResRef(string value) {
            return new CResRef(value);
        }

        public static implicit operator String(CResRef value) {
            return value.ToString();
        }

        public override string ToString() {
            return value;
        }

        public int CompareTo(object obj) {
            if (obj is CResRef) {
                CResRef other = (CResRef)obj;
                return value.CompareTo(other.value);
            } else {
                return 1;
            }
        }
    }
}
