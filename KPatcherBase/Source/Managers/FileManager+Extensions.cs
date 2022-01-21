using AppToolbox.Context;
using AppToolbox.Source.Managers;
using KPatcher.Installation;
using KPatcherBase.Source.Models.Install;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Managers {
    public static class FileManager_Extensions {
        public static ModInstallInfo loadModInstallInfo(this FileLoader fileManager, String filePath) {
            String fileText = File.ReadAllText(filePath);
            Dictionary<String, ModInstallOptionMap> obj = JsonConvert.DeserializeObject<Dictionary<String, ModInstallOptionMap>>(fileText);
            return new ModInstallInfo(obj);
        }
    }
}
