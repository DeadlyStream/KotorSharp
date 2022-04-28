using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static KPatcher.Source.Patcher.Patcher;

namespace KPatcher.Source.Patcher {
    internal static class NSSPatcher {
        public static void ProcessScriptSource(String nssText, PatchInfo patchInfo) {
            var matches = Regex.Matches(nssText, @"(?<=#)2DAMEMORY\d*(?=#)").DistinctBy((m) => m.Value);
            var newString = nssText;
            foreach(var match in matches) {
                newString = newString.Replace(String.Format("#{0}#", match.Value), patchInfo.tokenRegistry[match.Value]);
            }
            nssText = newString;
            return;
        }
    }
}
