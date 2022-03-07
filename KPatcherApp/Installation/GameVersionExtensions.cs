using KPatcherBase.Source.Models.Install;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Installation {
    static class GameVersionExtensions {
        public static GameInstallVersion toGameInstallVersion(this String s) {
            if (s.Equals("k1")) {
                return GameInstallVersion.K1;
            } else if (s.Equals("k2")) {
                return GameInstallVersion.K2;
            } else {
                return GameInstallVersion.Unknown;
            }
        }
    }
}
