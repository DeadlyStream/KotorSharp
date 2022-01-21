using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KotorManifest.Source.Constants {
    public static class KotorConstants {
        public static class Bifs {
            public static class _2da {
                public static readonly String BifKey = "data\\2da.bif";
                public static readonly String ItemPropDef2da = "itempropdef.2da";
                public static readonly String CostTable2da = "iprp_costtable.2da";
                public static readonly String ParamTable2da = "iprp_paramtable.2da";
            }
        }
        public static class Paths {
            public const String Data = "data";
            public const String Lips = "lips";
            public const String Modules = "modules";
            public const String Miles = "miles";
            public const String Movies = "movies";
            public const String Rims = "rims";
            public const String Override = "override";
            public const String Saves = "saves";
            public const String StreamMusic = "streammusic";
            public const String StreamSounds = "streamsounds";
            public const String StreamWaves = "streamwaves";
            public const String TexturePacks = "texturepacks";

            public static String[] allDirectories {
                get {
                    return new String[] {
                        Data,
                        Lips,
                        Modules,
                        Miles,
                        Movies,
                        Rims,
                        Override,
                        Saves,
                        StreamMusic,
                        StreamSounds,
                        StreamWaves,
                        TexturePacks
                    };
                }
            }

            public static String[] requiredDirectories {
                get {
                    return new String[] {
                        Data,
                        Lips,
                        Modules,
                        Movies,
                        Rims,
                        Override,
                        StreamMusic,
                        StreamSounds,
                        StreamWaves,
                        TexturePacks
                    };
                }
            }

            public static bool isKotorDirectory(String directory) {
                Regex r = new Regex(String.Format("(?i){0}", String.Join("|", allDirectories)));
                return r.Matches(directory).Count > 0;
            }
        }
    }
}
