using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Models {
    public class TlkStrRefEntry {
        public String text {
            get {
                return stringEntry;
            }
        }
        public double soundLength;
        public string soundResref;
        public string stringEntry;
        public bool textPresent;
        public bool soundResrefPresent;
        public bool soundLengthPresent;

        public TlkStrRefEntry(int flags, string soundResref, string stringEntry, double soundLength) {
            this.textPresent = (flags & 0x00000001) == 1;
            this.soundResrefPresent = (flags & 0x00000010) == 1;
            this.soundLengthPresent = (flags & 0x00000100) == 1;
            this.soundResref = soundResref;
            this.stringEntry = stringEntry;
            this.soundLength = soundLength;
        }

        public override string ToString() {
            return stringEntry;
        }

        public byte[] toBytes(int stringOffset) {
            ByteArray byteArray = new ByteArray();

            int flags = Convert.ToInt32(textPresent)
                | Convert.ToInt32(soundResrefPresent)
                | Convert.ToInt32(soundLengthPresent);

            byteArray.AddRange(BitConverter.GetBytes(flags));
            string soundResref = this.soundResref != null ? this.soundResref.PadRight(16, '\0').Substring(0, 16) : new string('\0', 16);
            byteArray.AddRange(Encoding.ASCII.GetBytes(soundResref));
            byteArray.AddRange(BitConverter.GetBytes(0));
            byteArray.AddRange(BitConverter.GetBytes(0));
            byteArray.AddRange(BitConverter.GetBytes(stringOffset));
            byteArray.AddRange(BitConverter.GetBytes(stringEntry.ToCharArray().Length));
            byteArray.AddRange(BitConverter.GetBytes((float)soundLength));
            return byteArray.ToArray();
        }
    }
}
