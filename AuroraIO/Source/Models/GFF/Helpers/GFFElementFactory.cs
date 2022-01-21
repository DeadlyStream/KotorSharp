using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;
using System.IO;

namespace AuroraIO
{
    public class GFFElementFactory {

        public static GFFStruct propertyStruct(ItemBlueprintPropertyInfo propertyInfo) {
            //For some reason, these need to be modified for use in uti files
            byte costTable = (byte)propertyInfo.costOptionRow;
            ushort costValue = (ushort)propertyInfo.costValue;
            byte param1 = (byte)propertyInfo.param1;
            Dictionary<String, GFFFieldDataObject> fieldMap = new Dictionary<string, GFFFieldDataObject>();
            fieldMap["ChanceAppear"] = new GFFByteDataObject(100);
            fieldMap["PropertyName"] = new GFFWordDataObject((ushort)propertyInfo.propertyValue);
            fieldMap["Subtype"] = new GFFWordDataObject((ushort)propertyInfo.subtypeValue);
            fieldMap["CostValue"] = new GFFWordDataObject(costValue);
            fieldMap["CostTable"] =  new GFFByteDataObject(costTable);
            fieldMap["Param1"] =  new GFFByteDataObject((byte)propertyInfo.param1Index);
            fieldMap["Param1Value"] =  new GFFByteDataObject(param1);
            return new GFFStruct(0, fieldMap);
        }
    }
}
