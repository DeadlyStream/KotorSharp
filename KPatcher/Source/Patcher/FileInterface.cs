using AuroraIO.Source.Archives.ERFRIM;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Dictionary;
using AuroraIO.Source.Models.Sound;
using AuroraIO.Source.Models.Table;
using AuroraIO.Source.Models.TLK;
using KPatcher.Source.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YAMLEncoding;

namespace KPatcher.Source.Patcher {
    public abstract class FileInterface {

        public abstract bool FileExists(string filePath);
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

        public void ExtractArchiveToDirectory(string sourceFilePath, string destinationFilePath) {
            var archive = ReadArchive(sourceFilePath);
            foreach (var entry in archive) {
                Write(Path.Combine(destinationFilePath, entry.name), entry.data, true);
            }
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

    public class DefaultFileInterface : FileInterface {

        public override bool FileExists(string filePath) {
            return File.Exists(filePath);
        }

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
            File.WriteAllText(filePath, text);  
        }

        public override string ReadText(string filePath) {
            return File.ReadAllText(filePath);
        }
    }

    public class VirtualFileInterface: FileInterface, YAMLEncodingProtocol, IDictionary<string, byte[]> {
        private Dictionary<string, byte[]> fileMap = new Dictionary<string, byte[]>();

        public ICollection<string> Keys => fileMap.Keys;

        public ICollection<byte[]> Values => fileMap.Values;

        public int Count => fileMap.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, byte[]>>)fileMap).IsReadOnly;

        public byte[] this[string key] { get => fileMap[key.ToLower()]; set => fileMap[key.ToLower()] = value; }

        public void LoadDirectory(string directory, bool overwrite = false) {
            foreach (var filePath in Directory.GetFiles(directory)) {
                LoadFile(filePath, overwrite);
            }
        }

        public void LoadFile(string file, bool overwrite = false) {
            Write(file, File.ReadAllBytes(file), overwrite);
        }

        public override bool FileExists(string filePath) {
            var key = filePath;
            return ContainsKey(key);
        }
        public override byte[] Read(string filePath) {
            return this[filePath];
        }

        public override void Write(string filePath, byte[] bytes, bool overwrite = false) {
            this[filePath] = bytes;
        }

        public override void Copy(string source, string destination, bool overwrite = false) {
            this[destination] = Read(source);
        }

        public override string ReadText(string filePath) {
            return Encoding.ASCII.GetString(Read(filePath));
        }

        public override void WriteText(string filePath, string text, bool overwrite = false) {
            Write(filePath, Encoding.ASCII.GetBytes(text), overwrite);
        }

        public string asciiEncoding(string indent = "") {
            StringBuilder sb = new StringBuilder();

            var sha = new SHA512Managed();

            var executingDirectory = Directory.GetCurrentDirectory();

            foreach (var pair in this) {
                sb.AppendFormat("-\n");
                sb.AppendFormat("  path: {0}\n", Path.GetRelativePath(executingDirectory, pair.Key));
                sb.AppendFormat("  data: {0}\n", Convert.ToBase64String(sha.ComputeHash(pair.Value)));
            }
            return sb.ToString();
        }

        public void Add(string key, byte[] value) {
            fileMap.Add(key, value);
        }

        public bool ContainsKey(string key) {
            return fileMap.ContainsKey(key);
        }

        public bool Remove(string key) {
            return fileMap.Remove(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out byte[] value) {
            return fileMap.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, byte[]> item) {
            ((ICollection<KeyValuePair<string, byte[]>>)fileMap).Add(item);
        }

        public void Clear() {
            ((ICollection<KeyValuePair<string, byte[]>>)fileMap).Clear();
        }

        public bool Contains(KeyValuePair<string, byte[]> item) {
            return ((ICollection<KeyValuePair<string, byte[]>>)fileMap).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, byte[]>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<string, byte[]>>)fileMap).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, byte[]> item) {
            return ((ICollection<KeyValuePair<string, byte[]>>)fileMap).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<string, byte[]>>)fileMap).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)fileMap).GetEnumerator();
        }
    }
}
