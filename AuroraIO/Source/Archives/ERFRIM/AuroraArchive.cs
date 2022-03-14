using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.ERFRIM {
    public class AuroraArchive: IEnumerable<AuroraFileEntry>, ASCIIEncodingProtocol {
        public struct Format {
            public static Format ERF = "ERF";
            public static Format HAK = "HAK";
            public static Format SAV = "SAV";
            public static Format MOD = "MOD";
            public static Format RIM = "RIM";

            private String value;

            private Format(String value) {
                this.value = value;
            }

            public static implicit operator Format(String value) {
                return new Format(value);
            }
            
            public static implicit operator String(Format format) {
                return format.value;
            }

            public override string ToString() {
                return value;
            }
        }

        public class Entry {
            public AuroraFileEntry file;
            Action deleteBlock;
            Action<byte[]> updateEntryBlock;
            internal Entry(AuroraFileEntry file, Action<byte[]> updateEntryBlock, Action deleteBlock) {
                this.file = file;
                this.deleteBlock = deleteBlock;
                this.updateEntryBlock = updateEntryBlock;
            }

            public void Update(Func<byte[], byte[]> updateDataBlock) {
                byte[] newData = updateDataBlock(file.data);
                updateEntryBlock(newData);
            }

            public void Delete() {
                deleteBlock();
            }
        }

        public uint descriptionStrRef;
        public CExoLocString localizedString;

        internal AuroraArchive(Format format, Dictionary<AuroraResourceName, byte[]> fileMap, uint descriptionStrRef = uint.MaxValue, CExoLocString localizedString = null) {
            this.fileMap = fileMap;
            this.format = format;
            this.descriptionStrRef = descriptionStrRef;
            this.localizedString = localizedString != null ? localizedString : new CExoLocString();
        }

        public int fileCount => fileMap.Count;

        private Dictionary<AuroraResourceName, byte[]> fileMap = new Dictionary<AuroraResourceName, byte[]>();
        public readonly Format format;

        public Entry Add(AuroraFileEntry file) {
            fileMap[file.name] = file.data;
            return Get(file.name);
        }

        public Entry Get(string key) {
            AuroraFileEntry file = new AuroraFileEntry(key, fileMap[key]);
            return new Entry(file, (newData) => {
                fileMap[file.name] = newData;
            },() => {
                fileMap.Remove(key);
            });
        }

        public string asciiEncoding(string indent = "") {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}format: {1}\n", indent, format.ToString());
            sb.AppendFormat("{0}descriptionStrRef: {1}\n", indent, descriptionStrRef);

            if (localizedString.languageCount > 0) {
                sb.AppendFormat("{0}localizedString:\n", indent);
            }
            foreach (KeyValuePair<CExoLanguage, string> stringPair in localizedString) {
                sb.AppendFormat("{0}  {1}: {2}\n", indent, stringPair.Key, stringPair.Value);
            }
            sb.AppendFormat("{0}files:\n", indent);
            foreach (KeyValuePair<AuroraResourceName, byte[]> pair in fileMap) {
                
                sb.AppendFormat("{0}  -\n", indent);
                sb.AppendFormat("{0}    name: {1}\n", indent, pair.Key);

                string fileText = Encoding.ASCII.GetString(pair.Value);

                if (fileText.All(c => { return char.IsWhiteSpace(c) || char.IsLetterOrDigit(c); })) {
                    sb.AppendFormat("{0}    value: {1}\n", indent, fileText);
                } else {
                    sb.AppendFormat("{0}    value: {1}\n", indent, BitConverter.ToString(pair.Value).Replace("-", ""));
                }
                    
            }

            return sb.ToString();
        }

        private List<AuroraFileEntry> files => fileMap.Select(pair => new AuroraFileEntry(pair.Key, pair.Value)).ToList();

        public IEnumerator<AuroraFileEntry> GetEnumerator() {
            return ((IEnumerable<AuroraFileEntry>)files).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)files).GetEnumerator();
        }
    }
}
