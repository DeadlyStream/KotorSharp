using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    static class ReservedWord {
        public static class Syntax {
            public const char Assign = '=';
            public const char Colon = ':';           
            public const String CommentEnd = "*/";
            public const String CommentStart = "/*";
            public const String Equal = "==";
            public const String LineComment = "//";
            public const String NotEqual = "!=";
        }
        public static class General {       
            public const string If = "if";        
            public const string Else = "else";
            public const string ElseIf = "elseif";
            public const string End = "end";
            public const string EndIf = "endif";
            public const string Opt = "opt";
            public const string SetVar = "setvar";
            public const string Var = "var";
            public const string Where = "where";
        }

        public static class FilePatch {
            public static class _2da {
                public const string AddRow = "add-row";
                public const string ModRow = "mod-row";
                public const string ClearRow = "clr-row";
                public const string RowIndex = "row-index";
            }

            public static class Tlk {
                public const string AddEntry = "add-entry";
                public const string ModEntry = "mod-entry";
                public const string LoadEntry = "load-entry";
            }
        }

        public static class Install {
            public const string Copy = "copy";
            public const string CopyFolder = "copy-folder";
            public const string CopyRename = "copy-rename";
        }

        public static class Package {
            public const string Install = "install";
            public const string Patch = "patch";
        }
    }
}
