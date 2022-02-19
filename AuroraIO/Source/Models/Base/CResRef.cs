using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Base {
    public class CResRef {

        public int Length => value.Length;

        private string value;
        private CResRef(string value) {
            this.value = value.Substring(0, Math.Min(value.Count(), 16));
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
    }
}
