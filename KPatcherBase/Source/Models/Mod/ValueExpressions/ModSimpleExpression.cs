using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models {
    class ModSimpleExpression: ModValueExpression {
        private string value;

        public ModSimpleExpression(string value) {
            this.value = value;
        }

        public string evaluatedValue() {
            return value;
        }

        public void store(string value) { }
    }
}
