using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraIO;

namespace KPatcher.Patching.Models.PatchCommands.Tlk {
    class TlkModEntryPatchCommand: ModFilePatchCommandExpression {
        private ModValueExpression expression;
        private Dictionary<GFFLanguage, string> tlkEntryMap;

        public TlkModEntryPatchCommand(ModValueExpression expression, Dictionary<GFFLanguage, string> tlkEntryMap) {
            this.expression = expression;
            this.tlkEntryMap = tlkEntryMap;
        }

        public string evaluatedValue() {
            throw new NotImplementedException();
        }

        public void store(string value) {
            throw new NotImplementedException();
        }
    }
}
