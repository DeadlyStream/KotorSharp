using AuroraIO.Source.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public class AuroraList : AuroraDataObject, ICollection<AuroraStruct> {
        public override AuroraDataType dataType => AuroraDataType.List;

        public int Count => ((ICollection<AuroraStruct>)structs).Count;

        public bool IsReadOnly => ((ICollection<AuroraStruct>)structs).IsReadOnly;

        private List<AuroraStruct> structs = new List<AuroraStruct>();

        public AuroraStruct this[int index]
        {
            get
            {
                return structs[index];
            }
            set
            {
                structs[index] = value;
            }
        }

        public AuroraList(AuroraStruct[] structs)
        {
            this.structs.AddRange(structs);
        }

        public override void setValueForKey(string key, string value)
        {
            List<string> components = value.Split('\\').ToList();
            components.RemoveAt(0);
            int index = Convert.ToInt32(components.First());
            string newKey = String.Join("\\", components);

            structs[index].setValueForKey(newKey, value);
        }

        public override string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}type: list\n", indent);
            sb.AppendFormat("{0}value:\n", indent);
            foreach (AuroraStruct arStruct in structs)
            {
                sb.AppendFormat("{0}  - struct:\n", indent);
                sb.AppendFormat("{0}    id: {1}\n", indent, arStruct.id);
                sb.AppendFormat("{0}    fields:\n", indent);
                foreach (KeyValuePair<CResRef, AuroraDataObject> pair in arStruct)
                {
                    sb.AppendFormat("{0}      {1}:\n", indent, pair.Key);
                    sb.Append(pair.Value.asciiEncoding(String.Format("{0}        ", indent)));
                }    
            }
            return sb.ToString();
        }

        public void Add(AuroraStruct item)
        {
            ((ICollection<AuroraStruct>)structs).Add(item);
        }

        public void Clear()
        {
            ((ICollection<AuroraStruct>)structs).Clear();
        }

        public bool Contains(AuroraStruct item)
        {
            return ((ICollection<AuroraStruct>)structs).Contains(item);
        }

        public void CopyTo(AuroraStruct[] array, int arrayIndex)
        {
            ((ICollection<AuroraStruct>)structs).CopyTo(array, arrayIndex);
        }

        public bool Remove(AuroraStruct item)
        {
            return ((ICollection<AuroraStruct>)structs).Remove(item);
        }

        public IEnumerator<AuroraStruct> GetEnumerator()
        {
            return ((IEnumerable<AuroraStruct>)structs).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)structs).GetEnumerator();
        }
    }
}
