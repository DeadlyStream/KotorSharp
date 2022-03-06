using AuroraIO.Source.Models.Base;
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

        public static byte[] GetBytes(AuroraString auroraData) {
            Data data = new Data();
            data.AddRange(BitConverter.GetBytes((UInt32)auroraData.value.Length));
            data.AddRange(Encoding.ASCII.GetBytes(auroraData.value));   
            return data;
        }

        public static byte[] GetBytes(AuroraResref auroraData) {
            Data data = new Data();
            data.Add((byte)auroraData.value.Length);
            data.AddRange(Encoding.ASCII.GetBytes(auroraData.value));
            return data;
        }

        public static byte[] GetBytes(AuroraLocalizedString auroraData) {
            Data data = new Data();

            Data substringArray = new Data();
            foreach (KeyValuePair<CExoLanguage, string> tuple in auroraData) {
                substringArray.AddRange(BitConverter.GetBytes((UInt32)tuple.Key));
                substringArray.AddRange(BitConverter.GetBytes((UInt32)tuple.Value.Length));
                substringArray.AddRange(Encoding.ASCII.GetBytes(tuple.Value));
            }

            data.AddRange(BitConverter.GetBytes((UInt32)substringArray.Count + 8));
            data.AddRange(BitConverter.GetBytes((UInt32)auroraData.strref));
            data.AddRange(BitConverter.GetBytes((UInt32)auroraData.Count()));
            data.AddRange(substringArray.ToArray());
            return data;
        }

        public static byte[] GetBytes(AuroraVoid auroraData) {
            Data data = new Data();
            data.AddRange(BitConverter.GetBytes((UInt32)auroraData.value.Length));
            data.AddRange(auroraData.value);
            return data;
        }

        public static byte[] GetBytes(AuroraQuaternion auroraData) {
            Data data = new Data();

            data.AddRange(BitConverter.GetBytes(auroraData.w));
            data.AddRange(BitConverter.GetBytes(auroraData.x));
            data.AddRange(BitConverter.GetBytes(auroraData.y));
            data.AddRange(BitConverter.GetBytes(auroraData.z));

            return data;
        }

        public static byte[] GetBytes(AuroraVector auroraData) {
            Data data = new Data();

            data.AddRange(BitConverter.GetBytes(auroraData.x));
            data.AddRange(BitConverter.GetBytes(auroraData.y));
            data.AddRange(BitConverter.GetBytes(auroraData.z));

            return data;
        }

        public static byte[] GetBytes(AuroraStrRef auroraData) { 
            Data data = new Data();
            data.AddRange(BitConverter.GetBytes(4));
            data.AddRange(BitConverter.GetBytes(auroraData.value));
            return data;
        }

        public static byte[] GetBytes(CResRef resref) {
            if (resref == null) return Encoding.ASCII.GetBytes(new string('\0', 16));
            string value = resref;
            return Encoding.ASCII.GetBytes(value.PadRight(16, '\0'));
        }
    }
}
