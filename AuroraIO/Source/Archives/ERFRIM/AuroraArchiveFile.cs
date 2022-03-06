using AuroraIO.Source.Coders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.ERFRIM {
    public static class AuroraArchiveFile {

        public static AuroraArchive CreateFromDirectory(string path, ERFRIMCoder.Format format = ERFRIMCoder.Format.Auto) {
            return null;
        }

        public static void ExtractToDirectory(String path, ERFRIMCoder.Format format = ERFRIMCoder.Format.Auto) {

        }

        public static AuroraArchive Load(string path, ERFRIMCoder.Format format = ERFRIMCoder.Format.Auto) {
            return new ERFRIMCoder().decode(File.ReadAllBytes(path));
        }

        public static void Write(AuroraArchive archive, string path, ERFRIMCoder.Format format = ERFRIMCoder.Format.Auto) {
            File.WriteAllBytes(path, new ERFRIMCoder().encode(archive, format));
        }
    }
}
