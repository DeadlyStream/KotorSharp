using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Extensions {
    internal static class Extensions {

        public static void SafeGetKey<T, K>(this IDictionary<T, K> dict, T key, Action<K> action) {
            if (dict.ContainsKey(key)) {
                action(dict[key]);
            }
        }
    }
}
