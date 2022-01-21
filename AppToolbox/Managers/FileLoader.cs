using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppToolbox.Source.Managers
{
    public class FileLoader {
        public Dictionary<String, Object> loadJsonObjectAtPath(String filePath) {
            return JsonConvert.DeserializeObject(File.ReadAllText(filePath)) as Dictionary<String, Object>;
        }
    }
}
