using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Constants {

    class Constants {

    }


    class KotorGameType {
        static class Constants {
            internal const string k1 = "k1";
            internal const string k2 = "k2";
        }
        internal static KotorGameType kotor1 = Constants.k1;
        internal static KotorGameType kotor2 = Constants.k2;

        String rawValue;
        private KotorGameType(String value) {
            switch (value) {
                case Constants.k1:
                    rawValue = value;
                    break;
                case Constants.k2:
                    rawValue = value;
                    break;
                default:
                    rawValue = null;
                    break;
            }
        }

        public static implicit operator KotorGameType(String value) {
            return new KotorGameType(value);
        }

        public static bool operator ==(KotorGameType lhs, KotorGameType rhs) {
            return (lhs?.rawValue ?? "").Equals(rhs?.rawValue ?? "");        
        }

        public static bool operator !=(KotorGameType lhs, KotorGameType rhs) {
            return !lhs.rawValue.Equals(rhs.rawValue);
        }

        public override int GetHashCode() {
            return rawValue.GetHashCode();
        }

        public override bool Equals(object obj) {
            return rawValue.Equals(((KotorGameType)obj).rawValue);
        }
    }
}
