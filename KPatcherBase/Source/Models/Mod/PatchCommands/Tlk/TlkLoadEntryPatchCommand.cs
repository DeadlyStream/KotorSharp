using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models.PatchCommands {
    public class TlkLoadEntryPatchCommand: ModFilePatchCommandExpression {
        private string tlkIdentifier;

        public TlkLoadEntryPatchCommand(string tlkIdentifier) {
            this.tlkIdentifier = tlkIdentifier;
        }

        public string evaluatedValue() {
            throw new NotImplementedException();
        }

        public void store(string value) {
            throw new NotImplementedException();
        }
    }
}
