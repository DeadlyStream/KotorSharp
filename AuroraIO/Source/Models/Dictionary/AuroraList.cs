using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public class AuroraList : AuroraDataObject {
        public AuroraDataType dataType { get { return AuroraDataType.List; } }

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
    }
}
