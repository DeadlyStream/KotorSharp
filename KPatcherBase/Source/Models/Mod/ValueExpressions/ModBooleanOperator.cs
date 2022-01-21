using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models {
    public enum ModBooleanOperator {
        Equals,
        NotEquals,
        Invalid
    }

    public static class KPBooleanComparerExtensions {
        public static string stringValue(this ModBooleanOperator boolComparer) {
            switch (boolComparer) {
                case ModBooleanOperator.Equals: return "==";
                case ModBooleanOperator.NotEquals: return "!=";
                default: return "invalid";
            }
        }

        public static ModBooleanOperator toBooleanComparer(this string s) {
            if (s == ModBooleanOperator.Equals.stringValue()) { return ModBooleanOperator.Equals; } else if (s == ModBooleanOperator.NotEquals.stringValue()) { return ModBooleanOperator.NotEquals; } else { return ModBooleanOperator.Invalid; }
        }

        public static bool boolValue(this ModBooleanOperator boolComparer) {
            switch (boolComparer) {
                case ModBooleanOperator.Equals: return true;
                case ModBooleanOperator.NotEquals: return false;
                default: return false;
            }
        }
    }
}
