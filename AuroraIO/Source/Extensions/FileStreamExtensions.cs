using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Extensions {
    public static class FileStreamExtensions {
        public static byte[] ReadAtOffset(this FileStream fileStream, int offset, int count) {
            byte[] data = new byte[count];
            fileStream.Seek(offset, SeekOrigin.Begin);
            fileStream.Read(data, 0, count);
            return data;
        }
    }
}
