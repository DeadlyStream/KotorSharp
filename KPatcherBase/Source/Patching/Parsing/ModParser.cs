using AppToolbox.Context;
using AppToolbox.Source.Managers;
using KPatcher.Installation;
using KPatcher.Installation.Paths;
using KPatcherBase.Models.Install;
using KPatcherBase.Source.Models.Install;
using KPatcherBase.Source.Models.Mod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public class ModParser {
        private static readonly ModParser instance = new ModParser();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ModParser() {
        }

        private ModParser() {
        }

        public static ModParser Instance {
            get {
                return instance;
            }
        }

        static class Constants {
            public const String KPatchFilter = "*.kpatch";
            public const String TSLPatcherIni = "*.ini";
        }
        
        /*
        public ModPackage[] loadModPackages(InstallInfo installInfo) {
            List<ModPackage> packages = new List<ModPackage>();
            foreach (KeyValuePair<String, ModInstallInfo> pair in installInfo.modInstallMap) {
                try {
                    String directoryPath = PatcherPath.Mods + "\\" + pair.Key;
                    Console.Write("Parsing {0}...", pair.Key);
                    packages.Add(loadModPackage(directoryPath, pair.Value));
                    Console.Write("Success\n");
                } catch (Exception e) {
                    Console.Write(String.Format("Failed\n {0}\n", e.Message));
                }
            }

            return packages.ToArray();
        }*/

        private ModPackage loadModPackage(String directoryPath, ModInstallInfo modInstallInfo) {
            String filePath = directoryPath + "\\mod.kpatch";
            ParsingContainer textContainer = ApplicationContext.Shared.fileLoader.loadModPackageText(filePath);
            String modName = textContainer.parseTerm();
            String modVersion = textContainer.parseTerm();
            textContainer.parseColon();

            List<ModPackageComponent> components = new List<ModPackageComponent>();
            while (!textContainer.tryParseEndMarker()) {
                components.Add(this.parsePackageComponent(textContainer));
            }

            return new ModPackage(directoryPath, modName, modVersion, components.ToArray());
        }
    }
}
