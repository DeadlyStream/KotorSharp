using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Base {
    public class CExoLocString : IEnumerable<KeyValuePair<CExoLanguage, string>> {

        public int languageCount => dict.Count;
        private Dictionary<CExoLanguage, String> dict;

        public static CExoLocString Empty = new CExoLocString();

        private CExoLocString(string value) {
            this.dict = new Dictionary<CExoLanguage, String>();
            dict[CExoLanguage.EnglishFemale] = value;
        }

        private CExoLocString(Dictionary<CExoLanguage, String> dict) {
            this.dict = dict;
        }

        public CExoLocString() {
            this.dict = new Dictionary<CExoLanguage, String>();
        }

        public string this[CExoLanguage key] {
            get {
                return dict[key];
            } set {
                dict[key] = value;
            }
        }

        public IEnumerator<KeyValuePair<CExoLanguage, string>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<CExoLanguage, string>>)dict).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)dict).GetEnumerator();
        }

        public static implicit operator CExoLocString(string value) {
            return new CExoLocString(value);
        }

        public static implicit operator CExoLocString(Dictionary<CExoLanguage, String> dict) {
            return new CExoLocString(dict);
        }
    }
}
