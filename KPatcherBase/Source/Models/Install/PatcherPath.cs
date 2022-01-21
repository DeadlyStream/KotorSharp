using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Models.Install {
    public class PatcherPath {
        public static String Root { get; private set; }
        public static String Mods { get; private set; }
        public static String UserInfo { get; private set; }

        static PatcherPath() {
            Root = Directory.GetCurrentDirectory();
            Mods = Root + "\\mods\\";
            UserInfo = Root + "\\userinfo\\";
        }
    }
}
