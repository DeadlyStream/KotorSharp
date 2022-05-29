using AuroraIO.Source.Coders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Patcher {
    public class TokenRegistry : IDictionary<string, string>, ASCIIEncodingProtocol {
        private Dictionary<string, string> internalDict = new Dictionary<string, string>();

        public string this[string key] { get => ((IDictionary<string, string>)internalDict)[key]; set => ((IDictionary<string, string>)internalDict)[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, string>)internalDict).Keys;

        public ICollection<string> Values => ((IDictionary<string, string>)internalDict).Values;

        public int Count => ((ICollection<KeyValuePair<string, string>>)internalDict).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, string>>)internalDict).IsReadOnly;

        public void Add(string key, string value) {
            ((IDictionary<string, string>)internalDict).Add(key, value);
        }

        public void Add(KeyValuePair<string, string> item) {
            ((ICollection<KeyValuePair<string, string>>)internalDict).Add(item);
        }

        public string asciiEncoding(string indent = "") {
            StringBuilder sb = new StringBuilder();
            foreach(var pair in internalDict) {
                sb.AppendFormat("- {0}: {1}\n", pair.Key, pair.Value);
            }
            
            return sb.ToString();
        }

        public void Clear() {
            ((ICollection<KeyValuePair<string, string>>)internalDict).Clear();
        }

        public bool Contains(KeyValuePair<string, string> item) {
            return ((ICollection<KeyValuePair<string, string>>)internalDict).Contains(item);
        }

        public bool ContainsKey(string key) {
            return ((IDictionary<string, string>)internalDict).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<string, string>>)internalDict).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<string, string>>)internalDict).GetEnumerator();
        }

        public bool Remove(string key) {
            return ((IDictionary<string, string>)internalDict).Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item) {
            return ((ICollection<KeyValuePair<string, string>>)internalDict).Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value) {
            return ((IDictionary<string, string>)internalDict).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)internalDict).GetEnumerator();
        }
    }
}
