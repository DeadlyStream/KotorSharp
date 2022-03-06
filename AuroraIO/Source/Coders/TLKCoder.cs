using AuroraIO.Models;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.TLK {
    public class TLKCoder {
        public TalkTable decode(byte[] byteArray) {
            TalkTable.LanguageID languageID = (TalkTable.LanguageID)BitConverter.ToUInt32(byteArray, 8);
            int stringCount = (int)BitConverter.ToUInt32(byteArray, 12);
            int stringEntriesOffset = (int)BitConverter.ToUInt32(byteArray, 16);

            int currentOffset = 20;
            TalkTable.Entry[] entries = new TalkTable.Entry[stringCount];
            for (int i = 0; i < stringCount; i++) {
                int flags = (int)BitConverter.ToUInt32(byteArray, currentOffset);
                bool textPresent = (flags & 1) == 1;
                bool soundResrefPresent = (flags & 2) == 2;
                bool soundLengthPresent = (flags & 4) == 4;

                string stringEntry = "";
                if (textPresent) {         
                    int offsetToString = (int)BitConverter.ToUInt32(byteArray, currentOffset + 28);
                    int stringSize = (int)BitConverter.ToUInt32(byteArray, currentOffset + 32);
                    stringEntry = Encoding.ASCII.GetString(byteArray, stringEntriesOffset + offsetToString, stringSize);
                }

                string soundResref = "";
                if (soundResrefPresent) {
                    soundResref = Encoding.ASCII.GetString(byteArray, currentOffset + 4, 16);
                }

                float soundLength = 0.0f;
                if (soundLengthPresent) {
                    soundLength = BitConverter.ToSingle(byteArray, currentOffset + 36);
                }            

                entries[i] = new TalkTable.Entry(stringEntry, soundResref, soundLength);
                currentOffset += 40;
            }
            return new TalkTable(languageID, entries);
        }

        public byte[] encode(TalkTable table) {
            Data data = new Data();

            //BuildHeader
            data.AddRange(Encoding.ASCII.GetBytes("TLK".ToUpper().PadRight(4)));
            data.AddRange(Encoding.ASCII.GetBytes("V3.0".PadRight(4)));
            data.AddRange(BitConverter.GetBytes((uint)table.language));

            int entryCount = table.Count;
            data.AddRange(BitConverter.GetBytes(entryCount));

            int stringEntriesOffset = 40 * entryCount + 20;
            data.AddRange(BitConverter.GetBytes(stringEntriesOffset));

            Data stringArray = new Data();
            foreach (TalkTable.Entry entry in table) {
                int flags = (entry.text.Length > 0 ? 1 : 0) 
                    | (entry.soundResref.Length > 0 ? 1 : 0) << 1
                    | (entry.soundLength > 0.0f ? 1 : 0) << 2;

                data.AddRange(BitConverter.GetBytes(flags));

                data.AddRange(AuroraBitConverter.GetBytes(entry.soundResref));
                
                data.AddRange(BitConverter.GetBytes(0));
                data.AddRange(BitConverter.GetBytes(0));
                data.AddRange(BitConverter.GetBytes(stringArray.Count));
                data.AddRange(BitConverter.GetBytes(entry.text.Length));
                data.AddRange(BitConverter.GetBytes(entry.soundLength));

                stringArray.AddRange(Encoding.ASCII.GetBytes(entry.text));
            }

            data.AddRange(stringArray);

            return data;
        }
    }
}
