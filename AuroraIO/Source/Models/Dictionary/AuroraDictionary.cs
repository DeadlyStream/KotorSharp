using AuroraIO.Models.Base;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public class AuroraDictionary : AuroraStructType, ASCIIEncodingProtocol {
        public string type { get; private set; }
        public uint structType => uint.MaxValue;

        private Dictionary<CResRef, AuroraDataObject> internalDict = new Dictionary<CResRef, AuroraDataObject>();

        AuroraDictionary(String type, Dictionary<CResRef, AuroraDataObject> dictionary)
        {
            this.type = type;
            internalDict = dictionary;
        }

        public static AuroraDictionary make(string type, Action<Dictionary<CResRef, AuroraDataObject>> initBlock)
        {
            Dictionary<CResRef, AuroraDataObject> dict = new Dictionary<CResRef, AuroraDataObject>();
            initBlock(dict);
            return new AuroraDictionary(type, dict);
        }

        public static AuroraDictionary make(Action<Dictionary<CResRef, AuroraDataObject>> initBlock)
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

            foreach (KeyValuePair<CResRef, AuroraDataObject> pair in internalDict.OrderBy( pair => pair.Key ))
            {
                sb.AppendFormat("  {0}:\n", pair.Key);
                sb.Append(pair.Value.asciiEncoding("    "));
            }
            return sb.ToString();
        }

        public IEnumerator<KeyValuePair<CResRef, AuroraDataObject>> GetEnumerator()
        {
            return internalDict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)internalDict).GetEnumerator();
        }
    }
}
