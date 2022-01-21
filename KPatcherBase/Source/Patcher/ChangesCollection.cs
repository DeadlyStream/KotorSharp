using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Patcher {
    public struct ChangesCollection {
        public LogLevel logLevel;

        public Dictionary<String, int> tlkList;

        public Dictionary<String, String[]> installList;

        public Dictionary<String, InstructionSet[]> twoDAList;

        public Dictionary<String, InstructionSet> gffList;

        public Dictionary<String, InstructionSet> compileList;

        public Dictionary<String, InstructionSet> ssfList;

        internal ChangesCollection(LogLevel logLevel, Dictionary<string, int> tlkList, Dictionary<string, string[]> installList, Dictionary<string, InstructionSet[]> twoDAList, Dictionary<string, InstructionSet> gffList, Dictionary<string, InstructionSet> compileList, Dictionary<string, InstructionSet> ssfList) {
            this.logLevel = logLevel;
            this.tlkList = tlkList;
            this.installList = installList;
            this.twoDAList = twoDAList;
            this.gffList = gffList;
            this.compileList = compileList;
            this.ssfList = ssfList;
        }
    }
}
