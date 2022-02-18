using AuroraIO.Source.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Coders {
    internal static class AuroraBitConverter {
        public static byte[] GetBytes(AuroraByte auroraData) {
            return BitConverter.GetBytes((uint)auroraData.value);
        }

        public static byte[] GetBytes(AuroraChar auroraData) {
            return BitConverter.GetBytes((uint)auroraData.value);
        }

        public static byte[] GetBytes(AuroraWord auroraData) {
            return BitConverter.GetBytes((uint)auroraData.value);
        }

        public static byte[] GetBytes(AuroraShort auroraData) {
            return BitConverter.GetBytes((uint)auroraData.value);
        }

        public static byte[] GetBytes(AuroraDWord auroraData) {
            return BitConverter.GetBytes(auroraData.value);
        }

        public static byte[] GetBytes(AuroraInt auroraData) {
            return BitConverter.GetBytes(auroraData.value);
        }

        public static byte[] GetBytes(AuroraDWord64 auroraData) {
            return BitConverter.GetBytes(auroraData.value);
        }

        public static byte[] GetBytes(AuroraInt64 auroraData) {
            return BitConverter.GetBytes(auroraData.value);
        }

        public static byte[] GetBytes(AuroraFloat auroraData) {
            return BitConverter.GetBytes(auroraData.value);
        }

        public static byte[] GetBytes(AuroraDouble auroraData) {
            return BitConverter.GetBytes(auroraData.value);
        }

        public static byte[] GetBytes(AuroraCExoString auroraData) {
            ByteArray byteArray = new ByteArray();
            byteArray.AddRange(BitConverter.GetBytes((UInt32)auroraData.value.Length));
            byteArray.AddRange(Encoding.ASCII.GetBytes(auroraData.value));   
            return byteArray.ToArray();
        }

        public static byte[] GetBytes(AuroraResref auroraData) {
            ByteArray byteArray = new ByteArray();
            byteArray.Add((byte)auroraData.value.Length);
            byteArray.AddRange(Encoding.ASCII.GetBytes(auroraData.value));
            return byteArray.ToArray();
        }

        public static byte[] GetBytes(AuroraCExoLocString auroraData) {
            ByteArray byteArray = new ByteArray();

            ByteArray substringArray = new ByteArray();
            foreach (KeyValuePair<CExoLanguage, string> tuple in auroraData) {
                substringArray.AddRange(BitConverter.GetBytes((UInt32)tuple.Key));
                substringArray.AddRange(BitConverter.GetBytes((UInt32)tuple.Value.Length));
                substringArray.AddRange(Encoding.ASCII.GetBytes(tuple.Value));
            }

            byteArray.AddRange(BitConverter.GetBytes((UInt32)substringArray.Count + 8));
            byteArray.AddRange(BitConverter.GetBytes((UInt32)auroraData.strref));
            byteArray.AddRange(BitConverter.GetBytes((UInt32)auroraData.Count()));
            byteArray.AddRange(substringArray.ToArray());
            return byteArray.ToArray();
        }

        public static byte[] GetBytes(AuroraVoid auroraData) {
            ByteArray byteArray = new ByteArray();
            byteArray.AddRange(BitConverter.GetBytes((UInt32)auroraData.value.Length));
            byteArray.AddRange(auroraData.value);
            return byteArray.ToArray();
        }

        public static byte[] GetBytes(AuroraQuaternion auroraData) {
            ByteArray byteArray = new ByteArray();

            byteArray.AddRange(BitConverter.GetBytes(auroraData.w));
            byteArray.AddRange(BitConverter.GetBytes(auroraData.x));
            byteArray.AddRange(BitConverter.GetBytes(auroraData.y));
            byteArray.AddRange(BitConverter.GetBytes(auroraData.z));

            return byteArray.ToArray();
        }

        public static byte[] GetBytes(AuroraVector auroraData) {
            ByteArray byteArray = new ByteArray();

            byteArray.AddRange(BitConverter.GetBytes(auroraData.x));
            byteArray.AddRange(BitConverter.GetBytes(auroraData.y));
            byteArray.AddRange(BitConverter.GetBytes(auroraData.z));

            return byteArray.ToArray();
        }

        public static byte[] GetBytes(AuroraStrRef auroraData) { 
            ByteArray byteArray = new ByteArray();
            byteArray.AddRange(BitConverter.GetBytes(4));
            byteArray.AddRange(BitConverter.GetBytes(auroraData.value));
            return byteArray.ToArray();
        }
    }
}
