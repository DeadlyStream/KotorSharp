using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Base {
    public struct CExoString {

        public int Length => value.Length;

        private string value;
        private CExoString(string value) {
            this.value = value;
        }

        public static implicit operator CExoString(string value) {
            return new CExoString(value);
        }

        public static implicit operator String(CExoString value) {
            return value.ToString();
        }

        public override string ToString() {
            return value;
        }
    }
}
