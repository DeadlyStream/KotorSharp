using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AuroraIO {
    public abstract class AuroraResource {
        public String fileName;
        public AuroraResourceType fileType;

        public AuroraResource() {
        }

        public AuroraResource(byte[] fileArray) : this (fileArray, null) {}

        public AuroraResource(byte[] fileArray, String fileName) {
            this.fileName = fileName;
        }

        public AuroraResource(String filePath) : this(File.ReadAllBytes(filePath), filePath.Split('/').Last()) {}

        public void writeToPath(String filePath) {
            writeToPath(filePath, false);
        }

        public void writeToPath(String filePath, bool addFileTypeExtension) {
            String writePath = filePath;

            if (addFileTypeExtension) {
                writePath += "." + fileType.stringValue();
            }
            
            File.WriteAllBytes(writePath, toBytes());   
        }

        public abstract byte[] toBytes();
    }
}
