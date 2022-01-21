using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models.PatchCommands {
    public class _2daAddRowPatchCommand : ModFilePatchCommandExpression {
        private Dictionary<string, string> patchValues;

        public _2daAddRowPatchCommand(Dictionary<string, string> patchValues) {
            this.patchValues = patchValues;
        }

        public string evaluatedValue() {
            throw new NotImplementedException();
        }

        public void store(string value) {
            throw new NotImplementedException();
        }
    }
}
