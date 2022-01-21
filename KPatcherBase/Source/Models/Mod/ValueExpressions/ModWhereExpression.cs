using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models {
    class ModWhereExpression: ModValueExpression {
        private ModBooleanExpression booleanExpression;
        private string calculatedValue;

        public ModWhereExpression(ModBooleanExpression booleanExpression) {
            this.booleanExpression = booleanExpression;
        }

        public string evaluatedValue() {
            return calculatedValue;
        }

        public void store(string value) {
            this.calculatedValue = value;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("where {0}", booleanExpression));
            return sb.ToString();
        }
    }
}
