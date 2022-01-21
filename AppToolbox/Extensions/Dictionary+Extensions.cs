using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppToolbox.Extensions {
    public static class Dictionary_Extensions {
        public static U safeGet<T, U>(this Dictionary<T, U> dict, T key) {
            if (dict.ContainsKey(key)) {
                return dict[key];
            } else {
                return default (U);
            }
        }
    }
}
