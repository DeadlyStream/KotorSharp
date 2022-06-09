using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAMLEncoding;

namespace AuroraIO.Source.Models.Dictionary {

    public abstract class AuroraDataObject: SetKeyValueInterface, YAMLEncodingProtocol {
        public abstract AuroraDataType dataType { get; }

        public abstract string asciiEncoding(string indent = "");
        public abstract void setValueForKey(KeyPath keyPath, string value);

        public static implicit operator AuroraDataObject(Int32 value)
        {
            return new AuroraInt(value);
        }

        public static implicit operator AuroraDataObject(byte b)
        {
            return new AuroraByte(b);
        }

        public static implicit operator AuroraDataObject(char value)
        {
            return new AuroraChar(value);
        }

        public static implicit operator AuroraDataObject(UInt16 value)
        {
            return new AuroraWord(value);
        }

        public static implicit operator AuroraDataObject(Int16 value)
        {
            return new AuroraShort(value);
        }

        public static implicit operator AuroraDataObject(UInt32 value)
        {
            return new AuroraDWord(value);
        }

        public static implicit operator AuroraDataObject(UInt64 value)
        {
            return new AuroraDWord64(value);
        }

        public static implicit operator AuroraDataObject(Int64 value)
        {
            return new AuroraInt64(value);
        }

        public static implicit operator AuroraDataObject(float value)
        {
            return new AuroraFloat(value);
        }

        public static implicit operator AuroraDataObject(double value)
        {
            return new AuroraDouble(value);
        }

        public static implicit operator AuroraDataObject(string value)
        {
            return new AuroraString(value);
        }

        public static implicit operator AuroraDataObject(byte[] byteArray)
        {
            return new AuroraVoid(byteArray);
        }

        public static implicit operator AuroraDataObject(AuroraStruct[] structs)
        {
            return new AuroraList(structs);
        }

        public static implicit operator AuroraDataObject((float, float, float, float) set)
        {
            return new AuroraQuaternion(set.Item1, set.Item2, set.Item3, set.Item4);
        }

        public static implicit operator AuroraDataObject((float, float, float) set)
        {
            return new AuroraVector(set.Item1, set.Item2, set.Item3);
        }
    }

    public class AuroraByte : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Byte;

        public byte value { get; private set; }

        public AuroraByte(byte value)
        {
            this.value = value;
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToByte(value);
        }
        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: byte\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraByte).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraChar: AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Char;

        public char value { get; private set; }

        public AuroraChar(char value)
        {
            this.value = value;
        }
        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToChar(value);
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: char\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraChar).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraWord : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Word;

        public UInt16 value { get; private set; }

        public AuroraWord(UInt16 value)
        {
            this.value = value;
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            try {
                this.value = Convert.ToUInt16(value);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: word\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraWord).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraShort : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Short;

        public Int16 value { get; private set; }

        public AuroraShort(Int16 value)
        {
            this.value = value;
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToInt16(value);
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: short\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraShort).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraDWord : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Dword;

        public UInt32 value { get; private set; }

        public AuroraDWord(UInt32 value)
        {
            this.value = value;
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToUInt32(value);
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: dword\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraDWord).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraInt : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Int;

        public Int32 value { get; private set; }

        public AuroraInt(Int32 value)
        {
            this.value = value;
        }
        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToInt32(value);
        }
        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: int\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraInt).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraDWord64 : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Dword64;

        public UInt64 value { get; private set; }

        public AuroraDWord64(UInt64 value)
        {
            this.value = value;
        }
        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToUInt64(value);
        }
        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: dword64\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraDWord64).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraInt64 : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Int64;

        public Int64 value { get; private set; }

        public AuroraInt64(Int64 value)
        {
            this.value = value;
        }
        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToInt64(value);
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: int64\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraInt64).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraFloat : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Float;

        public float value { get; private set; }

        public AuroraFloat(float value)
        {
            this.value = value;
        }
        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToSingle(value);
        }
        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: float\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraFloat).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraDouble : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Double;

        public double value { get; private set; }

        public AuroraDouble(double value)
        {
            this.value = value;
        }
        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToDouble(value);
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: double\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraDouble).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraString : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.CExoString;

        public CExoString value { get; private set; }
        public int Length => value.Length;
        public AuroraString(string value)
        {
            this.value = value;
        }
        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = value;
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: cexostring\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraString).value == value;
        }

        public override string ToString() {
            return value;
        }
    }

    public class AuroraResref : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.CResref;

        public CResRef value { get; private set; }

        AuroraResref(string value) {
            this.value = value;
        }

        public static AuroraResref make(string value) 
        {
            return new AuroraResref(value.Substring(0, Math.Min(value.Count(), 16)));
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = value.Substring(0, Math.Min(value.Count(), 16));
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: resref\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraResref).value == value;
        }

        public override string ToString() {
            return value;
        }
    }

    public class AuroraLocalizedString : AuroraDataObject, IEnumerable<KeyValuePair<CExoLanguage, string>> {
        public override AuroraDataType dataType => AuroraDataType.CExoLocString;
        public CExoLocString localizedString { get; private set; }
        public uint strref { get; private set; }

        private struct Keys
        {
            public const string Strref = "strref";
            public const string Lang = "lang";
        }

        public string this[CExoLanguage languageId]
        {
            get
            {
                return localizedString[languageId];
            } set
            {
                localizedString[languageId] = value;
            }
        }

        private AuroraLocalizedString(uint strref, CExoLocString localizedString)
        {
            this.strref = strref;
            this.localizedString = localizedString;
        }
             
        public static AuroraLocalizedString make(uint strref, Action<Dictionary<CExoLanguage, string>> initBlock)
        {
            Dictionary<CExoLanguage, string> dict = new Dictionary<CExoLanguage, string>();
            initBlock(dict);
            return new AuroraLocalizedString(strref, dict);
        }

        public static AuroraLocalizedString make(Action<Dictionary<CExoLanguage, string>> initBlock) {
            return AuroraLocalizedString.make(uint.MaxValue, initBlock);
        }

        public static AuroraLocalizedString make(uint strref)
        {
            return AuroraLocalizedString.make(strref, dict => { });
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            switch (keyPath)
            {
                case Keys.Lang:
                    localizedString[CExoLanguageFromString(keyPath)] = value;
                    break;
                case Keys.Strref:
                    this.strref = Convert.ToUInt32(value);
                    break;
            }
        }

        private CExoLanguage CExoLanguageFromString(string value)
        {
            int intValue = Convert.ToInt32(value.Replace("lang", ""));

            if (!Enum.IsDefined(typeof(CExoLanguage), intValue))
            {
                return CExoLanguage.Undefined;
            }

            return (CExoLanguage)intValue;
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: cexolocstring\n", indent);
            sb.AppendFormat("{0}value:\n", indent);
            sb.AppendFormat("{0}  strref: {1}\n", indent, (int)strref);

            foreach(KeyValuePair<CExoLanguage, string> pair in localizedString)
            {
                sb.AppendFormat("{0}  {1}: {2}\n", indent, pair.Key.stringValue(), pair.Value);
            }
            return sb.ToString();
        }

        public IEnumerator<KeyValuePair<CExoLanguage, string>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<CExoLanguage, string>>)localizedString).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)localizedString).GetEnumerator();
        }

        public override bool Equals(object obj) {
            var cexoLocString = obj as AuroraLocalizedString;
            return cexoLocString.localizedString == localizedString && cexoLocString.strref == strref;
        }
    }

    public class AuroraVoid : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Void;

        public byte[] value { get; private set; }

        public AuroraVoid(byte[] value)
        {
            this.value = value;
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Enumerable.Range(0, value.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                    .ToArray();
        }
        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: void\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, BitConverter.ToString(value).Replace("-", ""));
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraVoid).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public class AuroraQuaternion: AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Quaternion;

        public float w;
        public float x;
        public float y;
        public float z;

        public AuroraQuaternion(float w, float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            string[] values = value.Split('|');
            this.w = Convert.ToSingle(values.ElementAtOrDefault(0));
            this.x = Convert.ToSingle(values.ElementAtOrDefault(1));
            this.y = Convert.ToSingle(values.ElementAtOrDefault(2));
            this.z = Convert.ToSingle(values.ElementAtOrDefault(3));
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: quaternion\n", indent);
            sb.AppendFormat("{0}value:\n", indent);
            sb.AppendFormat("{0}  w: {1}\n", indent, w);
            sb.AppendFormat("{0}  x: {1}\n", indent, x);
            sb.AppendFormat("{0}  y: {1}\n", indent, y);
            sb.AppendFormat("{0}  z: {1}\n", indent, z);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            var quaternion = obj as AuroraQuaternion;
            return quaternion.w == w
                && quaternion.x == x
                && quaternion.y == y
                && quaternion.z == z;
        }

        public override string ToString() {
            return String.Format("{0} {1} {2} {3}", w, x, y, z);
        }
    }

    public class AuroraVector : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.Vector;

        public float x;
        public float y;
        public float z;

        public AuroraVector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public override void setValueForKey(KeyPath keyPath, string value)
        {
            string[] values = value.Split('|');
            this.x = Convert.ToSingle(values.ElementAtOrDefault(0));
            this.y = Convert.ToSingle(values.ElementAtOrDefault(1));
            this.z = Convert.ToSingle(values.ElementAtOrDefault(2));
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: vector\n", indent);
            sb.AppendFormat("{0}value:\n", indent);
            sb.AppendFormat("{0}  x: {1}\n", indent, x);
            sb.AppendFormat("{0}  y: {1}\n", indent, y);
            sb.AppendFormat("{0}  z: {1}\n", indent, z);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            var vector = obj as AuroraVector;
            return vector.x == x
                && vector.y == y
                && vector.z == z;
        }

        public override string ToString() {
            return String.Format("{0} {1} {2}", x, y, z);
        }
    }

    public class AuroraStrRef : AuroraDataObject
    {
        public override AuroraDataType dataType => AuroraDataType.StrRef;

        public UInt64 value { get; private set; }

        AuroraStrRef(UInt64 value)
        {
            this.value = value;
        }

        public static AuroraStrRef make(UInt64 id)
        {
            return new AuroraStrRef(id);
        }

        public override void setValueForKey(KeyPath keyPath, string value)
        {
            this.value = Convert.ToUInt64(value);
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: strref\n", indent);
            sb.AppendFormat("{0}value: {1}\n", indent, (int)value);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return (obj as AuroraStrRef).value == value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
