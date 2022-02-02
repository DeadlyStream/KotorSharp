using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public class AuroraStruct: AuroraDataObject {

        public AuroraDataType dataType { get { return AuroraDataType.Struct; } }

        public uint id;

        private Dictionary<string, AuroraDataObject> internalDict = new Dictionary<string, AuroraDataObject>();

        public AuroraStruct()
        {
            id = uint.MaxValue;
        }

        public AuroraStruct(uint id)
        {
            this.id = id;
        }

        public AuroraStruct(uint id, Dictionary<string, AuroraDataObject> dictionary)
        {
            this.id = id;
            internalDict = dictionary;
        }

        public static AuroraStruct make(Action<Dictionary<string, AuroraDataObject>> initBlock)
        {
            var instance = new AuroraStruct();
            initBlock(instance.internalDict);
            return instance;
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
    }
}
