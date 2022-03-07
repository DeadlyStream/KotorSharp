using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public class AuroraStruct: AuroraDataObject, AuroraStructType, ASCIIEncodingProtocol {

        public override AuroraDataType dataType => AuroraDataType.Struct;

        public uint structType => id;

        public uint id;

        private Dictionary<CResRef, AuroraDataObject> internalDict = new Dictionary<CResRef, AuroraDataObject>();

        AuroraStruct()
        {
            id = uint.MaxValue;
        }

        AuroraStruct(uint id)
        {
            this.id = id;
        }

        AuroraStruct(uint id, Dictionary<CResRef, AuroraDataObject> dictionary)
        {
            this.id = id;
            internalDict = dictionary;
        }

        public static AuroraStruct make(Action<Dictionary<CResRef, AuroraDataObject>> initBlock)
        {
            Dictionary<CResRef, AuroraDataObject> dict = new Dictionary<CResRef, AuroraDataObject>();
            initBlock(dict);
            return new AuroraStruct(uint.MaxValue, dict);
        }

        public static AuroraStruct make(uint id, Action<Dictionary<CResRef, AuroraDataObject>> initBlock)
        {
            Dictionary<CResRef, AuroraDataObject> dict = new Dictionary<CResRef, AuroraDataObject>();
            initBlock(dict);
            return new AuroraStruct(id, dict);
        }

        public static AuroraStruct make(uint id)
        {
            return new AuroraStruct(id);
        }

        public override void setValueForKey(string key, string value)
        {
            List<string> components = value.Split('\\').ToList();
            components.RemoveAt(0);
            string thisKey = components.First();
            string newKey = String.Join("\\", components);

            internalDict[thisKey].setValueForKey(newKey, value);
        }
        public AuroraDataObject this[String key]
        {
            get
            {
                return internalDict[key];
            } set
            {
                internalDict[key] = value;
            }
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: struct\n", indent);
            sb.AppendFormat("{0}value:\n", indent);
            sb.AppendFormat("{0}  id: {1}\n", indent, (int)id);
            sb.AppendFormat("{0}  fields:\n", indent);

            foreach (KeyValuePair<CResRef, AuroraDataObject> pair in internalDict) {
                sb.AppendFormat("{0}    {1}:\n", indent, pair.Key);
                sb.Append(pair.Value.asciiEncoding(String.Format("{0}      ", indent)));
            }
            return sb.ToString();
        }

        public IEnumerator<KeyValuePair<CResRef, AuroraDataObject>> GetEnumerator()
        {
            return internalDict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
