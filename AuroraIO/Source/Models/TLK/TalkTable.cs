using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Models {
    public class TalkTable: AuroraResource, ICollection<TlkStrRefEntry> {
        private List<TlkStrRefEntry> entries;
        private string fileVersion;
        private uint languageID;

        public TalkTable(String filePath) {
            byte[] fileArray = File.ReadAllBytes(filePath);
            this.fileType = Encoding.ASCII.GetString(fileArray, 0, 4).Replace(" ", "").ToLower().toAuroraResourceType();
            this.fileVersion = Encoding.ASCII.GetString(fileArray, 4, 4);
            this.languageID = BitConverter.ToUInt32(fileArray, 8);
            int stringCount = (int)BitConverter.ToUInt32(fileArray, 12);
            int stringEntriesOffset = (int)BitConverter.ToUInt32(fileArray, 16);

            int currentOffset = 20;
            Dictionary<int, TlkStrRefEntry> entries = new Dictionary<int, TlkStrRefEntry>();
            for (int i = 0; i < stringCount; i++) {
                int flags = (int)BitConverter.ToUInt32(fileArray, currentOffset);
                bool textPresent = (flags & 0x00000001) == 1;
                bool soundResrefPresent = (flags & 0x00000010) == 1;
                bool soundLengthPresent = (flags & 0x00000100) == 1;
                string soundResref = Encoding.ASCII.GetString(fileArray, currentOffset + 4, 16);
                int offsetToString = (int)BitConverter.ToUInt32(fileArray, currentOffset + 28);
                int stringSize = (int)BitConverter.ToUInt32(fileArray, currentOffset + 32);

                string stringEntry = Encoding.ASCII.GetString(fileArray, stringEntriesOffset + offsetToString, stringSize);
                double soundLength = BitConverter.ToDouble(fileArray, currentOffset + 36);
                entries[i] = new TlkStrRefEntry(flags, soundResref, stringEntry, soundLength);
                currentOffset += 40;
            }
            this.entries = entries.Values.ToList();
        }

        public int Count {
            get {
                return ((ICollection<TlkStrRefEntry>)entries).Count;
            }
        }

        public bool IsReadOnly {
            get {
                return ((ICollection<TlkStrRefEntry>)entries).IsReadOnly;
            }
        }

        public TlkStrRefEntry this[int index] {
            get {
                return entries.ElementAt(index);
            }
        }

        public void Add(TlkStrRefEntry item) {
            ((ICollection<TlkStrRefEntry>)entries).Add(item);
        }

        public int addEntry(String stringEntry, string soundResRef, double soundLength) {
            bool textPresent = stringEntry != null && stringEntry.Length > 0;
            bool soundResrefPresent = soundResRef != null;
            bool soundLengthPresent = soundLength != double.MaxValue;

            int entryCount = entries.Count + 1;

            int flags = Convert.ToInt32(textPresent)
                & Convert.ToInt32(soundResrefPresent)
                & Convert.ToInt32(soundLengthPresent);

            entries[entryCount] = new TlkStrRefEntry(flags, soundResRef, stringEntry, soundLength);
            return entries.Count;
        }

        public void Clear() {
            ((ICollection<TlkStrRefEntry>)entries).Clear();
        }

        public bool Contains(TlkStrRefEntry item) {
            return ((ICollection<TlkStrRefEntry>)entries).Contains(item);
        }

        public void CopyTo(TlkStrRefEntry[] array, int arrayIndex) {
            ((ICollection<TlkStrRefEntry>)entries).CopyTo(array, arrayIndex);
        }

        public IEnumerator<TlkStrRefEntry> GetEnumerator() {
            return ((ICollection<TlkStrRefEntry>)entries).GetEnumerator();
        }

        public bool Remove(TlkStrRefEntry item) {
            return ((ICollection<TlkStrRefEntry>)entries).Remove(item);
        }

        public override byte[] toBytes() {
            ByteArray byteArray = new ByteArray();

            //BuildHeader
            byteArray.AddRange(Encoding.ASCII.GetBytes(fileType.stringValue().ToUpper().PadRight(4)));
            byteArray.AddRange(Encoding.ASCII.GetBytes(fileVersion.PadRight(4)));
            byteArray.AddRange(BitConverter.GetBytes(languageID));

            int stringCount = entries.Count;
            byteArray.AddRange(BitConverter.GetBytes(stringCount));

            int stringEntriesOffset = 40 * entries.Count + 20;
            byteArray.AddRange(BitConverter.GetBytes(stringEntriesOffset));

            int stringOffset = 0;
            foreach(TlkStrRefEntry entry in entries) {
                byteArray.AddRange(entry.toBytes(stringOffset));
                stringOffset += entry.text.ToCharArray().Length;
            }

            foreach (TlkStrRefEntry entry in entries) {
                byteArray.AddRange(Encoding.ASCII.GetBytes(entry.text.ToCharArray()));
            }

            return byteArray.ToArray();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((ICollection<TlkStrRefEntry>)entries).GetEnumerator();
        }
    }
}
