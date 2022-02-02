using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary {
    public abstract class AuroraDataObject {
        AuroraDataType dataType { get; }


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
            return new AuroraCExoString(value);
        }

        public static implicit operator AuroraDataObject(byte[] byteArray)
        {
            return new AuroraVoid(byteArray);
        }

        public static implicit operator AuroraDataObject(AuroraStruct[] structs)
        {
            return new AuroraList(structs);
        }

        public static implicit operator AuroraDataObject(Dictionary<string, AuroraDataObject> dictionary)
        {
            return new AuroraStruct(uint.MaxValue, dictionary);
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
        public AuroraDataType dataType { get { return AuroraDataType.Byte; } }

        public byte value { get; }

        public AuroraByte(byte value)
        {
            this.value = value;
        }
    }

    public class AuroraChar: AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Char; } }

        public char value { get; }

        public AuroraChar(char value)
        {
            this.value = value;
        }
    }

    public class AuroraWord : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Word; } }

        public UInt16 value { get; }

        public AuroraWord(UInt16 value)
        {
            this.value = value;
        }
    }

    public class AuroraShort : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Short; } }

        public Int16 value { get; }

        public AuroraShort(Int16 value)
        {
            this.value = value;
        }
    }

    public class AuroraDWord : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Dword; } }

        public UInt32 value { get; }

        public AuroraDWord(UInt32 value)
        {
            this.value = value;
        }
    }

    public class AuroraInt : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Int; } }

        public Int32 value { get; }

        public AuroraInt(Int32 value)
        {
            this.value = value;
        }
    }

    public class AuroraDWord64 : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Dword64; } }

        public UInt64 value { get; }

        public AuroraDWord64(UInt64 value)
        {
            this.value = value;
        }
    }

    public class AuroraInt64 : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Int64; } }

        public Int64 value { get; }

        public AuroraInt64(Int64 value)
        {
            this.value = value;
        }
    }

    public class AuroraFloat : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Float; } }

        public float value { get; }

        public AuroraFloat(float value)
        {
            this.value = value;
        }
    }

    public class AuroraDouble : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Double; } }

        public double value { get; }

        public AuroraDouble(double value)
        {
            this.value = value;
        }
    }

    public class AuroraCExoString : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.CExoString; } }

        public string value { get; }

        public AuroraCExoString(string value)
        {
            this.value = value;
        }
    }

    public class AuroraResref : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.CResref; } }

        public string value { get; }

        AuroraResref(string value)
        {
            this.value = value;
        }

        public static AuroraResref make(string value) 
        {
            return new AuroraResref(value.Substring(0, Math.Min(value.Count(), 16)));
        }
    }

    public class AuroraCExoLocString : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.CExOoLocString; } }
        private Dictionary<CExoLanguage, string> dictionary = new Dictionary<CExoLanguage, string>();

        public string this[CExoLanguage languageId]
        {
            get
            {
                return dictionary[languageId];
            } set
            {
                dictionary[languageId] = value;
            }
        }
             
        public static AuroraCExoLocString make(Action<Dictionary<CExoLanguage, string>> initBlock)
        {
            var instance = new AuroraCExoLocString();
            initBlock(instance.dictionary);
            return instance;
        }
    }

    public class AuroraVoid : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Void; } }

        public byte[] value { get; }

        public AuroraVoid(byte[] byteArray)
        {
            this.value = value;
        }
    }

    public class AuroraQuaternion: AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Quaternion; } }

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
    }

    public class AuroraVector : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.Vector; } }

        public float x;
        public float y;
        public float z;

        public AuroraVector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class AuroraStrRef : AuroraDataObject
    {
        public AuroraDataType dataType { get { return AuroraDataType.StrRef; } }

        public UInt64 value { get; }

        AuroraStrRef(UInt64 value)
        {
            this.value = value;
        }

        public static AuroraStrRef make(UInt64 id)
        {
            return new AuroraStrRef(id);
        }
    }
}
