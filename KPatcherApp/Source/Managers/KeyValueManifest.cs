using AppToolbox.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Managers {
    public class KeyValueManifest {
        Dictionary<String, Object> map = new Dictionary<string, object>();

        public object this[String key] {
            get {
                if (map.ContainsKey(key)) {
                    return map[key];
                } else {
                    return null;
                }
            } set {
                Log.debugLine(String.Format("manifest.{0} = {1}", key, value));
                map[key] = value;
            }
        }
    }
}
