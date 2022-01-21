using KPatcher.Patching.Models;
using KPatcher.Patching.Models.PatchCommands;
using KPatcher.Patching.Parsing;
using KPatcherBase.Source.Models.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public static class _2daFilePatchParsingExtensions {
        public static ModFilePatchCommandExpression parse2daFilePatchCommand(this ModParser modParser,
                                                                             ParsingContainer textContainer)
        {
            String word = textContainer.parseTerm();

            switch (word) {
                case ReservedWord.FilePatch._2da.AddRow:
                    return modParser.parseAdd2daRowCommand(textContainer);
                case ReservedWord.FilePatch._2da.ModRow:
                    return modParser.parseMod2daRowCommand(textContainer);
                case ReservedWord.FilePatch._2da.ClearRow:
                    ModValueExpression expression = modParser.parseValueExpression(textContainer);
                    return new _2daClearRowPatchCommand(expression);
                case ReservedWord.General.Var:
                    return modParser.parsePatchVar(textContainer);
                case ReservedWord.General.SetVar:
                    return modParser.parsePatchVar(textContainer);
                case ReservedWord.General.If:
                    return modParser.parseIf(textContainer);
                default:
                    return null;
            }
        }

        private static ModFilePatchCommandExpression parseAdd2daRowCommand(this ModParser modParser,
                                                                           ParsingContainer patchText)
        {
            Dictionary<String, String> patchValues = new Dictionary<String, String>();
            patchText.parseColon();
            while (!patchText.tryParseEndMarker()) {
                String columnLabel = patchText.parseTerm();
                String columnValue = patchText.parseTerm();
                patchValues[columnLabel] = columnValue;
            }
            return new _2daAddRowPatchCommand(patchValues);
        }

        private static ModFilePatchCommandExpression parseMod2daRowCommand(this ModParser modParser,
                                                                           ParsingContainer patchText)
        {
            ModValueExpression expression = modParser.parseValueExpression(patchText);

            Dictionary<String, String> patchValues = new Dictionary<String, String>();
            patchText.parseColon();
            while (!patchText.tryParseEndMarker()) {
                String columnLabel = patchText.parseTerm();
                String columnValue = patchText.parseTerm();
                patchValues[columnLabel] = columnValue;
            }
            return new _2daModRowPatchCommand(expression, patchValues);
        }

        private static ModFilePatchCommandExpression parseRowIndex2daRowCommand(this ModParser modParser,
                                                                                ParsingContainer patchText)
        {
            ModValueExpression expression = modParser.parseValueExpression(patchText);
            return new _2daRowIndexPatchCommand(expression);
        }

        private static ModVar parsePatchVar(this ModParser modParser, ParsingContainer patchText) {
            String varName = patchText.parseTerm();
            patchText.tryParseEqualAssignment();

            String term = patchText.peekTerm();
            ModValueExpression expression;
            switch (term) {
                case ReservedWord.FilePatch._2da.AddRow:
                case ReservedWord.FilePatch._2da.ModRow:
                case ReservedWord.FilePatch._2da.RowIndex:
                    expression = modParser.parse2daFilePatchCommand(patchText);
                    break;
                default:
                    expression = modParser.parseValueExpression(patchText);
                    break;
            }

            return new ModVar(varName, expression);
        }
    }
}
