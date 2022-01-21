using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraIO;

namespace KPatcher.Patching.Models.PatchCommands.Tlk {
    class TlkAddEntryPatchCommand: ModFilePatchCommandExpression {
        private Dictionary<GFFLanguage, string> tlkEntryMap;

        public TlkAddEntryPatchCommand(Dictionary<GFFLanguage, string> tlkEntryMap) {
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
