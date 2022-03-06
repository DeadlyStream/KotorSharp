using AuroraIO.Source.Coders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.ERFRIM {
    public static class AuroraArchiveFile {

        public static AuroraArchive CreateFromDirectory(string path, AuroraArchiveCoder.Format format = AuroraArchiveCoder.Format.Auto) {
            return null;
        }

        public static void ExtractToDirectory(String path, AuroraArchiveCoder.Format format = AuroraArchiveCoder.Format.Auto) {

        }

        public static AuroraArchive Load(string path, AuroraArchiveCoder.Format format = AuroraArchiveCoder.Format.Auto) {
            return new AuroraArchiveCoder().decode(File.ReadAllBytes(path));
        }

        public static void Write(AuroraArchive archive, string path, AuroraArchiveCoder.Format format = AuroraArchiveCoder.Format.Auto) {
            File.WriteAllBytes(path, new AuroraArchiveCoder().encode(archive, format));
        }
    }
}
