using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.TLK {
    public class TalkTable: ICollection<TalkTable.Entry>, ASCIIOutputProtocol {

        public enum LanguageID {
            English = 0,
            French = 1,
            German = 2,
            Italian = 3,
            Spanish = 4,
            Polish = 5,
            Korean = 128,
            ChineseTraditional = 129,
            ChineseSimplified = 130,
            Japanese = 131
        }

        public class Entry {
            public CExoString text;
            public CResRef soundResref;
            public float soundLength;

            public Entry(
                string text,
                string soundResref,
                float soundLength
            ) {
                this.text = text == null ? "" : text;
                this.soundResref = soundResref == null ? "" : soundResref;
                this.soundLength = soundLength;
            }
        }

        private List<Entry> entries;

        public readonly LanguageID language;

        public TalkTable(LanguageID language, Entry[] entries) {
            this.language = language;
            this.entries = new List<Entry>(entries);
        }

        public TalkTable(LanguageID language) {
            this.language = language;
            this.entries = new List<Entry>();
        }

        public int Count => ((ICollection<Entry>)entries).Count;

        public bool IsReadOnly => ((ICollection<Entry>)entries).IsReadOnly;

        public Entry this[int index] {
            get {
                return entries[index];
            } set {
                entries[index] = value;
            }
        }

        public void Add(Entry item) {
            ((ICollection<Entry>)entries).Add(item);
        }

        public void Clear() {
            ((ICollection<Entry>)entries).Clear();
        }

        public bool Contains(Entry item) {
            return ((ICollection<Entry>)entries).Contains(item);
        }

        public void CopyTo(Entry[] array, int arrayIndex) {
            ((ICollection<Entry>)entries).CopyTo(array, arrayIndex);
        }

        public bool Remove(Entry item) {
            return ((ICollection<Entry>)entries).Remove(item);
        }

        public IEnumerator<Entry> GetEnumerator() {
            return ((IEnumerable<Entry>)entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)entries).GetEnumerator();
        }

        string ASCIIOutputProtocol.asciiEncoding(string indent) {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("language: {0}\n", language);
            sb.Append("entries:");

            for (int i = 0; i < entries.Count; i++) {
                TalkTable.Entry entry = entries[i];
                sb.AppendFormat("\n  - {0}", i);

                if (entry.text.Length > 0) {
                    sb.Append("\n    text: |");
                    sb.AppendFormat("\n      {0}", entry.text);
                }

                if (entry.soundResref.Length > 0) {
                    sb.AppendFormat("\n    soundResref: {0}", entry.soundResref);
                }

                if(entry.soundLength > 0.0f) {
                    sb.AppendFormat("\n    soundLength: {0}", entry.soundLength.ToString("G"));
                }
            }

            return sb.ToString();
        }
    }
}
