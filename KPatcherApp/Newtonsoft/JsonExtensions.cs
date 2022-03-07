using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Newtonsoft {
    using JsonDict = Dictionary<String, Object>;
    public static class JsonExtensions {
        static JObject toJObject(this Object o) {
            return (JObject)o;
        }

        public static JArray toJArray(this Object o) {
            return (JArray)o;
        }

        public static Dictionary<String, Object> toJsonDict(this Object o) {
            return o.toJObject().ToObject<Dictionary<String, Object>>();
        }

        public static Dictionary<String, Object>[] toJsonDictArray(this Object o) {
            return o.toJArray().ToObject<Dictionary<String, Object>[]>();
        }

        public static int toJSONInt(this Object o) {
            return Convert.ToInt32(o);
        }

        public static Int64 toJSONInt64(this Object o) {
            return Convert.ToInt64(o);
        }

        public static UInt64 toJSONUInt64(this Object o) {
            return Convert.ToUInt64(o);
        }

        public static float toJSONFloat(this Object o) {
            return Convert.ToSingle(o);
        }

        public static double toJSONDouble(this Object o) {
            return Convert.ToDouble(o);
        }

        public static string toJSONString(this Object o) {
            return o.ToString();
        }
    }
}
