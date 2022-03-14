using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Ini {
    internal class IniSection: IDictionary<string, string> {
        private Dictionary<string, string> values = new Dictionary<string, string>();

        public IniSection() {}

        public string this[string key] { get => ((IDictionary<string, string>)values)[key]; set => ((IDictionary<string, string>)values)[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, string>)values).Keys;

        public ICollection<string> Values => ((IDictionary<string, string>)values).Values;

        public int Count => ((ICollection<KeyValuePair<string, string>>)values).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, string>>)values).IsReadOnly;

        public void Add(string key, string value) {
            ((IDictionary<string, string>)values).Add(key, value);
        }

        public void Add(KeyValuePair<string, string> item) {
            ((ICollection<KeyValuePair<string, string>>)values).Add(item);
        }

        public void Clear() {
            ((ICollection<KeyValuePair<string, string>>)values).Clear();
        }

        public bool Contains(KeyValuePair<string, string> item) {
            return ((ICollection<KeyValuePair<string, string>>)values).Contains(item);
        }

        public bool ContainsKey(string key) {
            return ((IDictionary<string, string>)values).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<string, string>>)values).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<string, string>>)values).GetEnumerator();
        }

        public bool Remove(string key) {
            return ((IDictionary<string, string>)values).Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item) {
            return ((ICollection<KeyValuePair<string, string>>)values).Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value) {
            return ((IDictionary<string, string>)values).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)values).GetEnumerator();
        }

        public static implicit operator Dictionary<string, string>(IniSection iniSection) {
            return iniSection.values;
        }
    }
}
