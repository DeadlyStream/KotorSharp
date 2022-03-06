using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Coders {
    public class SSFCoder {
        public byte[] encode(AuroraSoundSet soundSet) {
            Data data = new Data();

            data.AddRange(Encoding.ASCII.GetBytes("SSF "));
            data.AddRange(Encoding.ASCII.GetBytes("v1.1"));
            data.AddRange(BitConverter.GetBytes((uint)12));

            foreach (uint strref in soundSet) {
                data.AddRange(BitConverter.GetBytes(strref));
            }
            
            return data;
        }

        public AuroraSoundSet decode(Data data) {
            int offset = (int)BitConverter.ToUInt32(data, 8);
            List<uint> entries = new List<uint>();
            while(offset < data.Count) {
                uint strref = BitConverter.ToUInt32(data, offset);
                entries.Add(strref);
                offset += 4;
            }

            return new AuroraSoundSet(entries);
        }
    }
}
