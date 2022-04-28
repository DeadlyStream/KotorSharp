using KPatcher.Source.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Patcher {
    internal static class DictionaryExtensions {
        internal static Dictionary<K, V> Filter<K, V>(this Dictionary<K, V> dict, Func<K, V, bool> block) {
            return dict.Where((pair) => {
                return block(pair.Key, pair.Value);
            }).ToDictionary(k => k.Key, v => v.Value);
        }

        internal static IniSection Filter(this IniSection iniSection, Func<string, string, bool> block) {
            Dictionary<string, string> dict = iniSection;
            return iniSection.Filter(block);
        }
    }
}
