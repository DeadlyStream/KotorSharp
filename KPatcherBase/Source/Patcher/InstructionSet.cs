using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Patcher {
    public struct InstructionSet {
        public Dictionary<String, String> keyValueSet { get; private set; }
        public InstructionSet[] instructions { get; private set; }

        public InstructionSet(Dictionary<String, String> keyValueSet) {
            this.keyValueSet = keyValueSet;
            this.instructions = new InstructionSet[0];
        }

        public InstructionSet(Dictionary<String, String> keyValueSet, InstructionSet[] instructions) {
            this.keyValueSet = keyValueSet;
            this.instructions = instructions;
        }
    }
}
