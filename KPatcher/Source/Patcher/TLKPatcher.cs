using AuroraIO.Source.Models.TLK;
using KPatcher.Source.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KPatcher.Source.Patcher.Patcher;

namespace KPatcher.Source.Patcher {
    internal static class TLKPatcher {
        public static void Process(Dictionary<string, string> values, TalkTable dialogTLK, TalkTable appendTLK, TokenRegistry tokenRegistry) {
            foreach(var pair in values) {
                var entry = appendTLK[int.Parse(pair.Value)];
                tokenRegistry[pair.Key] = dialogTLK.Count.ToString();
                dialogTLK.Add(entry);
            }
        }
    }
}
