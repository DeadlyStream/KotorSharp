using AuroraIO;
using KPatcher.Patching.Models;
using KPatcher.Patching.Models.PatchCommands;
using KPatcher.Patching.Models.PatchCommands.Tlk;
using KPatcherBase.Source.Models.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public static class TLKFilePatchParsingExtensions {
        public static ModFilePatchCommandExpression parseTlkFilePatchCommand(this ModParser modParser,
                                                                             ParsingContainer textContainer)
        {
            String word = textContainer.parseTerm();

            switch (word) {
                case ReservedWord.FilePatch.Tlk.AddEntry:
                    return modParser.parseTlkAddEntryCommand(textContainer);
                case ReservedWord.FilePatch.Tlk.ModEntry:
                    return modParser.parseTlkModEntryCommand(textContainer);
                case ReservedWord.FilePatch.Tlk.LoadEntry:
                    return modParser.parseTlkLoadEntryCommand(textContainer);
                case ReservedWord.General.Var:
                    return modParser.parsePatchVar(textContainer);
                case ReservedWord.General.SetVar:
                    return modParser.parsePatchVar(textContainer);
                case ReservedWord.General.If:
                    return modParser.parseIf(textContainer);
                default:
                    throw new Exception(String.Format("Unexpected command type for tlk commands: {0}", word));
            }
        }

        private static ModFilePatchCommandExpression parseTlkLoadEntryCommand(this ModParser modParser,
                                                                              ParsingContainer textContainer)
        {
            String tlkIdentifier = textContainer.parseTerm();
            return new TlkLoadEntryPatchCommand(tlkIdentifier);
        }

        private static ModFilePatchCommandExpression parseTlkModEntryCommand(this ModParser modParser,
                                                                             ParsingContainer textContainer)
        {
            ModValueExpression expression = modParser.parseValueExpression(textContainer);
            textContainer.parseColon();
            Dictionary<GFFLanguage, String> tlkEntryMap = new Dictionary<GFFLanguage, string>();
            while(textContainer.tryParseEndMarker()) {
                GFFLanguage language = textContainer.parseTerm().toGFFLanguage();
                String text = textContainer.parseTerm();
                tlkEntryMap[language] = text;
            }
            return new TlkModEntryPatchCommand(expression, tlkEntryMap);
        }

        private static ModFilePatchCommandExpression parseTlkAddEntryCommand(this ModParser modParser,
                                                                             ParsingContainer textContainer)
        {
            Dictionary<GFFLanguage, String> tlkEntryMap = new Dictionary<GFFLanguage, string>();
            textContainer.parseColon();
            while (!textContainer.tryParseEndMarker()) {
                GFFLanguage language = textContainer.parseTerm().toGFFLanguage();
                String text = textContainer.parseTerm();
                tlkEntryMap[language] = text;
            }
            return new TlkAddEntryPatchCommand(tlkEntryMap);
        }

        private static ModVar parsePatchVar(this ModParser modParser,
            ParsingContainer textContainer)
        {
            String varName = textContainer.parseTerm();
            textContainer.tryParseEqualAssignment();

            String term = textContainer.peekTerm();
            ModValueExpression expression;
            switch (term) {
                case ReservedWord.FilePatch.Tlk.AddEntry:
                case ReservedWord.FilePatch.Tlk.ModEntry:
                case ReservedWord.FilePatch.Tlk.LoadEntry:
                    expression = modParser.parseTlkFilePatchCommand(textContainer);
                    break;
                default:
                    expression = modParser.parseValueExpression(textContainer);
                    break;
            }

            return new ModVar(varName, expression);
        }
    }
}
