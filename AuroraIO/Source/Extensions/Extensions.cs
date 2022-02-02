using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO {
    internal static class Extensions {
        internal static string GetNullTerminatedString(this Encoding encoding, byte[] byteArray, int start) {
            StringBuilder s = new StringBuilder();
            int i = start;
            char c = (char)9;
            while (c != 0) {
                c = (char)byteArray[i++];
                if (c != 0) {
                    s.Append(c);
                }
            }
            return s.ToString();
        }

        internal static string sanitize(this string value)
        {
            if (value == null) { return null; }
            var regex = new Regex("[\n\r\t]*");
            return regex.Replace(value, "");
        }

        internal static IndexMap<T> generateIndexMap<T>(this T[] array) {
            return new IndexMap<T>(array);
        }

        internal static byte[] GetBytes(String stringValue) {
            byte[] byteArray = new byte[stringValue.Length / 2];
            int index = 0;
            while (stringValue.Length > 0) {
                String byteString = stringValue.Substring(0, 2);
                stringValue = stringValue.Substring(2, stringValue.Length - 2);
                byteArray[index] = Convert.ToByte(byteString, 16);
                index++;
            }
            return byteArray;
        }
    }

    public class ByteArray: List<byte> { }
}
