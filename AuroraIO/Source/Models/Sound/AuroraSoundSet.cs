using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Sound {
    public class AuroraSoundSet: IEnumerable<uint>, ASCIIEncodingProtocol {

        public enum Entry {
            BattleCry1 = 0,
            BattleCry2,
            BattleCry3,
            BattleCry4,
            BattleCry5,
            BattleCry6,
            Select1,
            Select2,
            Select3,
            AttackGrunt1,
            AttackGrunt2,
            AttackGrunt3,
            PainGrunt1,
            PainGrunt2,
            LowHealth,
            Dead,
            CriticalHit,
            TargetImmune,
            LayMine,
            DisarmMine,
            BeginStealth,
            BeginSearch,
            BeginUnlock,
            UnlockFailed,
            UnlockSuccess,
            SeparatedFromParty,
            RejoinParty,
            Poisoned
        }

        private uint[] entries = Enumerable.Repeat(uint.MaxValue, 32).ToArray();

        public AuroraSoundSet(List<uint> entries) {
            this.entries = entries.ToArray();
        }

        public uint this[Entry entry] {
            get {
                return entries[(int)entry];
            }
            set {
                entries[(int)entry] = value;
            }
        }

        public AuroraSoundSet() {}

        public string asciiEncoding(string indent = "") {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: ssf\n", indent);
            foreach (uint strref in entries) {    
                sb.AppendFormat("{0}  - {1}\n", indent, strref);
            }

            return sb.ToString();
        }

        public IEnumerator<uint> GetEnumerator() {
            return ((IEnumerable<uint>)entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return entries.GetEnumerator();
        }
    }
}
