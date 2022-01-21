using KPatcherBase.Models.Install;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Installation.Paths
{
    public class GamePath
    {
        public static String Root { get; private set; }
        public static String Data { get; private set; }
        public static string Rims { get; private set; }
        public static string Modules { get; private set; }
        public static string Override { get; private set; }

        static GamePath()
        {
            string patcherPath = PatcherPath.Root;
            Root = patcherPath.Replace("\\kpatcher", "");
            Data = Root + "\\data";
            Modules = Root + "\\modules";
            Rims = Root + "\\rims";
            Override = Root + "\\override";
        }
    }
}
