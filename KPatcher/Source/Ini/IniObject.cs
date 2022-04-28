using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Ini {
    public class IniObject: IDictionary<string, IniSection> {
        private Dictionary<string, IniSection> internalDict = new Dictionary<string, IniSection>();

        public IniSection this[string key] { get => internalDict.ContainsKey(key) ? internalDict[key] : new IniSection(); set => internalDict[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, IniSection>)internalDict).Keys;

        public ICollection<IniSection> Values => ((IDictionary<string, IniSection>)internalDict).Values;

        public int Count => ((ICollection<KeyValuePair<string, IniSection>>)internalDict).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, IniSection>>)internalDict).IsReadOnly;

        public void Add(string key, IniSection value) {
            ((IDictionary<string, IniSection>)internalDict).Add(key, value);
        }

        public void Add(KeyValuePair<string, IniSection> item) {
            ((ICollection<KeyValuePair<string, IniSection>>)internalDict).Add(item);
        }

        public void Clear() {
            ((ICollection<KeyValuePair<string, IniSection>>)internalDict).Clear();
        }

        public bool Contains(KeyValuePair<string, IniSection> item) {
            return ((ICollection<KeyValuePair<string, IniSection>>)internalDict).Contains(item);
        }

        public bool ContainsKey(string key) {
            return ((IDictionary<string, IniSection>)internalDict).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, IniSection>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<string, IniSection>>)internalDict).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, IniSection>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<string, IniSection>>)internalDict).GetEnumerator();
        }

        public bool Remove(string key) {
            return ((IDictionary<string, IniSection>)internalDict).Remove(key);
        }

        public bool Remove(KeyValuePair<string, IniSection> item) {
            return ((ICollection<KeyValuePair<string, IniSection>>)internalDict).Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out IniSection value) {
            return ((IDictionary<string, IniSection>)internalDict).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)internalDict).GetEnumerator();
        }
    }
}
