using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    internal class IndexMap<T> : ICollection<T> {
        private Dictionary<T, int> internalMap = new Dictionary<T, int>();

        public int Count => internalMap.Keys.Count;

        public bool IsReadOnly => false;

        private List<T> orderedKeys => internalMap.OrderBy(pair => pair.Value).Select(pair => pair.Key).ToList();

        public T this[int index] {
            get {
                return orderedKeys[index];
            }
        }

        public int this[T key] {
            get {
                return internalMap[key];
            }
        }

        public IndexMap() {

        }

        public IndexMap(T[] items) {
            internalMap = items.Select((value, index) => new { value, index })
                    .ToDictionary(pair => pair.value, pair => pair.index);
        }

        public void Add(T item) {
            if (!internalMap.ContainsKey(item)) {
                internalMap[item] = internalMap.Keys.Count;
            }
        }

        public int IndexOf(T item) {
            if (internalMap.ContainsKey(item)) {
                return internalMap[item];
            } else {
                return -1;
            }
        }

        public void Clear() {
            internalMap = new Dictionary<T, int>();
        }

        public bool Contains(T item) {
            return internalMap.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            orderedKeys.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            return orderedKeys.GetEnumerator();
        }

        public bool Remove(T item) {
            return internalMap.Remove(item);
        }

        public T[] toArray() {
            return orderedKeys.ToArray();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return orderedKeys.GetEnumerator();
        }
    }
}
