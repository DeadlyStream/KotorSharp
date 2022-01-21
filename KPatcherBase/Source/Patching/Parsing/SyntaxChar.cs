using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    class SyntaxChar {
    }


    static class SyntaxCharExtensions {
        public static bool isNextCharColon(this String s) {
            return s[0] == ReservedWord.Syntax.Colon;
        }

        public static String removeFirstChar(this String s) {
            return s.Remove(0, 1);
        }
    }
}
