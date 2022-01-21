using KPatcher.Patching.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Mod {
    public enum ModIfStatementType {
        If,
        ElseIf,
        Else,
        Endif
    }

    static class ModIfStatementTypeExtensions {
        public static ModIfStatementType toModIfStatementType(this string s) {
            if (ModIfStatementType.If.stringValue() == s) { return ModIfStatementType.If; }
            else if (ModIfStatementType.ElseIf.stringValue() == s) { return ModIfStatementType.ElseIf; }
            else if (ModIfStatementType.Else.stringValue() == s) { return ModIfStatementType.Else; }
            else if (ModIfStatementType.Endif.stringValue() == s) { return ModIfStatementType.Endif; }
            else {
                throw new Exception("Expected `if` `else if` `else` or `endif`");
            }
        }

        public static string stringValue(this ModIfStatementType type) {
            switch (type) {
                case ModIfStatementType.If: return ReservedWord.General.If;
                case ModIfStatementType.ElseIf: return ReservedWord.General.ElseIf;
                case ModIfStatementType.Else: return ReservedWord.General.Else;
                case ModIfStatementType.Endif: return ReservedWord.General.EndIf;
                default:
                    throw new Exception("This should be unreachable");
            }
        }
    }
}
