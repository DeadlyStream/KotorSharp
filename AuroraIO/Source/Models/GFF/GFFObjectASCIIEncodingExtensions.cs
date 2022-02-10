using AuroraIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.GFF {
    public static class GFFObjectASCIIEncodingExtensions {

        public static string asciiEncoding(this GFFStruct gffStruct) {
            uint structType = gffStruct.structType;
            Dictionary<String, GFFFieldDataObject> fields = gffStruct.fields;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("struct"));
            sb.AppendLine(String.Format("id {0}", structType));
            
            foreach(KeyValuePair<String, GFFFieldDataObject> pair in gffStruct.fields) {
                sb.AppendLine(String.Format("field {0}", pair.Key));
                sb.AppendLine(String.Format("type {0}", pair.Value.fieldType().stringValue()));
                sb.AppendLine(String.Format("data {0}", pair.Value.asciiEncoding("")));
            }
            sb.AppendLine("end");
            sb.AppendLine("end");
            return sb.ToString();
        }
    }
}
