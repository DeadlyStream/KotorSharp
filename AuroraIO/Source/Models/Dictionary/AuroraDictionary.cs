using AuroraIO.Source.Coders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public class AuroraDictionary : AuroraStructType, ASCIIOutputProtocol, SetKeyValueInterface {
        public string type { get; private set; }
        public uint structType => uint.MaxValue;

        private Dictionary<string, AuroraDataObject> internalDict = new Dictionary<string, AuroraDataObject>();

        AuroraDictionary(String type, Dictionary<string, AuroraDataObject> dictionary)
        {
            this.type = type;
            internalDict = dictionary;
        }

        public static AuroraDictionary make(string type, Action<Dictionary<string, AuroraDataObject>> initBlock)
        {
            Dictionary<string, AuroraDataObject> dict = new Dictionary<string, AuroraDataObject>();
            initBlock(dict);
            return new AuroraDictionary(type, dict);
        }

        public static AuroraDictionary make(Action<Dictionary<string, AuroraDataObject>> initBlock)
        {
            return AuroraDictionary.make("gff", initBlock);
        }

        public void setValueForKey(string key, string value)
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
            }
            set
            {
                internalDict[key] = value;
            }
        }

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("type: {0}\n", type);
            sb.AppendFormat("fields:\n");

            foreach (KeyValuePair<string, AuroraDataObject> pair in internalDict)
            {
                sb.AppendFormat("  {0}:\n", pair.Key);
                sb.Append(pair.Value.asciiEncoding("    "));
            }
            return sb.ToString();
        }

        public IEnumerator<KeyValuePair<string, AuroraDataObject>> GetEnumerator()
        {
            return internalDict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)internalDict).GetEnumerator();
        }
    }
}
