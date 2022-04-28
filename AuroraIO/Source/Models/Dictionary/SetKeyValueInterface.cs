using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Source.Models.Dictionary
{
    public interface SetKeyValueInterface {
        void setValueForKey(KeyPath keyPath, string value);
    }

    public class KeyPath {
        public bool HasSubPaths => keyPath.Count > 1;
        private List<string> keyPath;

        private KeyPath(string keyPath) {
            this.keyPath = keyPath.Split(new char[] { '\\', '(', ')' }).ToList();
        }

        public static implicit operator KeyPath(string keyPath) {
            return new KeyPath(keyPath);
        }

        public static implicit operator String(KeyPath keyPath) {
            return String.Join("\\", keyPath);
        }

        public string Pop() {
            var path = keyPath.First();
            keyPath.RemoveAt(0);
            return path;
        }

        public string Peek() {
            return keyPath.First();
        }
    }
}
