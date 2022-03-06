using AuroraIO.Models.Base;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.BIFKey {
    public class BIFArchive: ASCIIOutputProtocol {

        private class ResourceEntry {

        }

        private Dictionary<AuroraResourceName, uint> fileMap;
        private string filePath;
        private FileStream stream;

        private bool isLoaded => stream != null;

        internal BIFArchive(Dictionary<AuroraResourceName, uint> fileMap, string filePath) {
            this.fileMap = fileMap;
            this.filePath = filePath;
        }

        public void Load() {
            stream = File.OpenRead(filePath);
        }

        public void Close() {
            stream.Close();
        }

        public string asciiEncoding(string indent = "") {
            StringBuilder sb = new StringBuilder();

            foreach(var pair in fileMap) {
                sb.AppendFormat("{0}-\n", indent);
                sb.AppendFormat("{0}  id: {1}\n", indent, pair.Value);
                sb.AppendFormat("{0}  fileName: {1}\n", indent, pair.Key);
            }
            return sb.ToString();
        }

        public AuroraFile extract(string fileName) {
            if (!isLoaded) Load();

            int variableResourceCount = (int)BitConverter.ToUInt32(stream.ReadAtOffset(8, 4), 0);
            int variableTableOffset = (int)BitConverter.ToUInt32(stream.ReadAtOffset(16, 4), 0);

            int readingOffset = variableTableOffset;

            uint resID = fileMap[fileName];

            for (int i = 0; i < variableResourceCount; i++) {
                int offset = readingOffset + i * 16;
                uint id = BitConverter.ToUInt32(stream.ReadAtOffset(offset, 4), 0);

                if (resID == id) {
                    int fileDataOffset = (int)BitConverter.ToUInt32(stream.ReadAtOffset(offset + 4, 4), 0);
                    int fileDataSize = (int)BitConverter.ToUInt32(stream.ReadAtOffset(offset + 8, 4), 0);

                    byte[] filedata = new byte[fileDataSize];
                    stream.Seek(fileDataOffset, SeekOrigin.Begin);
                    stream.Read(filedata, 0, fileDataSize);

                    return new AuroraFile(fileName, filedata);
                }
            }

            return null;
        }

        public AuroraFile[] extractAll() {
            if (!isLoaded) Load();

            int variableResourceCount = (int)BitConverter.ToUInt32(stream.ReadAtOffset(8, 4), 0);
            int variableTableOffset = (int)BitConverter.ToUInt32(stream.ReadAtOffset(16, 4), 0);

            int readingOffset = variableTableOffset;

            Dictionary<uint, byte[]> loadedFiles = new Dictionary<uint, byte[]>();

            for (int i = 0; i < variableResourceCount; i++) {
                int offset = readingOffset + i * 16;

                uint id = BitConverter.ToUInt32(stream.ReadAtOffset(offset, 4), 0);

                int fileDataOffset = (int)BitConverter.ToUInt32(stream.ReadAtOffset(offset + 4, 4), 0);
                    int fileDataSize = (int)BitConverter.ToUInt32(stream.ReadAtOffset(offset + 8, 4), 0);

                byte[] filedata = stream.ReadAtOffset(fileDataOffset, fileDataSize);

                loadedFiles[id] = filedata;
            }

            return fileMap.ToDictionary(pair => pair.Key, pair => {
                return loadedFiles[pair.Value];
            }).Select(pair => {
                return new AuroraFile(pair.Key, pair.Value);
            }).ToArray();
        }

        public void ExtractToDirectory(string path) {

        }
    }
}
