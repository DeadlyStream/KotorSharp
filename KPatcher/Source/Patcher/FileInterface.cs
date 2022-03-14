using AuroraIO.Source.Archives.ERFRIM;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Dictionary;
using AuroraIO.Source.Models.Sound;
using AuroraIO.Source.Models.Table;
using AuroraIO.Source.Models.TLK;
using KPatcher.Source.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Patcher {
    internal abstract class FileInterface {
        public abstract byte[] Read(string filePath);

        public abstract void Write(string filePath, byte[] bytes, bool overwrite = false);

        public abstract void Copy(string source, string destination, bool overwrite = false);

        public abstract string ReadText(string filePath);

        public abstract void WriteText(string filePath, string text, bool overwrite = false);

        public TalkTable ReadTLK(string filePath) {
            return new TLKCoder().decode(Read(filePath));
        }

        public void WriteTLK(string filePath, TalkTable tlk, bool overwrite = false) {
            Write(filePath, new TLKCoder().encode(tlk), overwrite);
        }

        public AuroraArchive ReadArchive(string filePath) {
            return new ERFRIMCoder().decode(Read(filePath));
        }

        public void WriteArchive(string filePath, AuroraArchive archive, bool overwrite = false) {
            Write(filePath, new ERFRIMCoder().encode(archive), overwrite);
        }

        public AuroraTable Read2DA(string filePath) {
            return new _2DACoder().decode(Read(filePath));
        }

        public void Write2DA(string filePath, AuroraTable table, bool overwrite = false) {
            Write(filePath, new _2DACoder().encode(table), overwrite);
        }

        public AuroraDictionary ReadGFF(string filePath) {
            return new GFFCoder().decode(Read(filePath));
        }

        public void WriteGFF(string filePath, AuroraDictionary dict, bool overwrite = false) {
            Write(filePath, new GFFCoder().encode(dict), overwrite);
        }

        public AuroraSoundSet ReadSSF(string filePath) {
            return new SSFCoder().decode(Read(filePath));
        }

        public void WriteSSF(string filePath, AuroraSoundSet ssf, bool overwrite = false) {
            Write(filePath, new SSFCoder().encode(ssf), overwrite);
        }
    }

    internal class DefaultFileInterface : FileInterface {
        public override byte[] Read(string filePath) {
            return File.ReadAllBytes(filePath);
        }

        public override void Write(string filePath, byte[] bytes, bool overwrite = false) {
            File.WriteAllBytes(filePath, bytes);
        }

        public override void Copy(string source, string destination, bool overwrite = false) {
            File.Copy(source, destination, overwrite);
        }

        public override void WriteText(string filePath, string text, bool overwrite = false) {
            if (overwrite) {
                File.WriteAllText(filePath, text);
            }  
        }

        public override string ReadText(string filePath) {
            return File.ReadAllText(filePath);
        }
    }

    internal class VirtualFileInterface: FileInterface {
        public Dictionary<string, byte[]> fileMap = new Dictionary<string, byte[]>();
        public override byte[] Read(string filePath) {
            if (fileMap.ContainsKey(filePath)) {
                return fileMap[filePath];
            } else {
                return File.ReadAllBytes(filePath);
            }
        }

        public override void Write(string filePath, byte[] bytes, bool overwrite = false) {
            fileMap[filePath] = bytes;
        }

        public override void Copy(string source, string destination, bool overwrite = false) {
            fileMap[destination] = Read(source);
        }

        public override string ReadText(string filePath) {
            return File.ReadAllText(filePath);
        }

        public override void WriteText(string filePath, string text, bool overwrite = false) {
            File.WriteAllText(filePath, text);
        }
    }
}
