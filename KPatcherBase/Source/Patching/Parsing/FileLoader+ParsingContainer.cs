using AppToolbox.Source.Managers;
using KPatcher.Installation;
using KPatcher.Installation.Paths;
using KPatcher.Patching.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching {
    public static class FileLoaderExtensions {
        public static ParsingContainer loadModPackageText(this FileLoader fileLoader,
            String filePath)
        {
            return new ParsingContainer(File.ReadAllText(filePath));
        }

        public static ParsingContainer loadTSLPatcherText(this FileLoader fileLoader,
            String filePath) {
            return new ParsingContainer(File.ReadAllText(filePath));
        }
    }
}
