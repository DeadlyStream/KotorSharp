using AuroraIO;
using KPatcher.Patching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public static class ModFilePatchCommandParserExtensions {
        public static ModFilePatchCommand parseFilePatchCommand(this ModParser modParser,
                                                                ParsingContainer textContainer,
                                                                AuroraResourceType resourceType)
        {
            switch (resourceType) {
                case AuroraResourceType.TwoDA:
                    return modParser.parse2daFilePatchCommand(textContainer);
                case AuroraResourceType.TLK:
                    return modParser.parseTlkFilePatchCommand(textContainer);//
                case AuroraResourceType.UTI:
                    return null;// KPGFFPatchCommandParser.parseGFFFilePatchCommand(patchText);
                case AuroraResourceType.UTC:
                    return null;// KPGFFPatchCommandParser.parseGFFFilePatchCommand(patchText);
                case AuroraResourceType.GFF:
                    return null;// KPGFFPatchCommandParser.parseGFFFilePatchCommand(patchText);
                default:
                    throw new Exception(String.Format("File commands not implemented for resource type {0}", resourceType));
            }
        }
    }
}
