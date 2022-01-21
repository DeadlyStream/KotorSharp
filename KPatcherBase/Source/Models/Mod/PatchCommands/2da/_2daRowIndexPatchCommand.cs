using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models.PatchCommands {
    class _2daRowIndexPatchCommand : ModFilePatchCommandExpression {
        private ModValueExpression expression;

        public _2daRowIndexPatchCommand(ModValueExpression expression) {
            this.expression = expression;
        }

        public string evaluatedValue() {
            return expression.evaluatedValue();
        }

        public void store(string value) {
            expression.store(value);
        }
    }
}
