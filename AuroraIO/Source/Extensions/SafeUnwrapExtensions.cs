using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Extensions {
    public static class SafeUnwrapExtensions {
        public static void Let<T>(this T t, Action<T> action) {
            if (t != null) {
                action(t);
            }
        }
    }
}
