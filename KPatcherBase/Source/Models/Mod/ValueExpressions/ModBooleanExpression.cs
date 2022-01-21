using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models {

    public class ModBooleanExpression {
        private ModBooleanOperator boolOperator;
        private String lhs;
        private String rhs;

        public ModBooleanExpression(String lhs, String rhs, ModBooleanOperator boolOperator) {
            this.lhs = lhs;
            this.rhs = rhs;
            this.boolOperator = boolOperator;
        }

        public bool evaluation() {
            if (rhs == null) {
                return lhs != 0.ToString() || lhs != "false" || lhs.Length > 0;
            } else {
                return lhs == rhs;
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0} {1} {2}", lhs, boolOperator.stringValue(), rhs));
            return sb.ToString();
        }
    }
}
