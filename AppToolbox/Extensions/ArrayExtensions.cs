using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppToolbox.Extensions {
    public static class ArrayExtensions {
        public static T safeGetValue<T>(this T[] array, int index) {
            if (index < array.Length) {
                return array[index];
            } else {
                return default(T);
            }
        }
    }
}
