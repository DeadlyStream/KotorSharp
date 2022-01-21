using KPatcher.Patching.Models;
using KPatcherBase.Source.Models.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public static class ModIfStatementParsingExtensions {
        public static String parseIfStatement(this ParsingContainer textContainer) {
            String word = textContainer.parseTerm();
            switch (word) {
                case ReservedWord.General.If:
                case ReservedWord.General.ElseIf:
                case ReservedWord.General.Else:
                case ReservedWord.General.EndIf:
                    return word;
                default:
                throw new Exception("Expected `if` `else if` `else` or `endif`");
            }
        }

        public static ModPackageComponent[] parseIfInstructions(this ModParser modParser, ParsingContainer textContainer) {
            List<ModPackageComponent> components = new List<ModPackageComponent>();
            while (!textContainer.tryParseEndIfMarker()) {
                components.Add(modParser.parsePackageComponent(textContainer));
            }
            return components.ToArray();
        }

        public static ModIfStatement parseIf(this ModParser modParser, ParsingContainer patchText) {
            Dictionary<ModIfStatementClause, ModPackageComponent[]> ifMap = new Dictionary<ModIfStatementClause, ModPackageComponent[]>();

            ModIfStatementType ifType = ModIfStatementType.If;
            while (ifType != ModIfStatementType.Endif) {
                ModIfStatementClause ifStatement = modParser.parseIfStatementClause(ifType, patchText);
                patchText.parseColon();
                while (!patchText.tryParseEndIfMarker()) {
                    ModPackageComponent[] instructions = modParser.parseIfInstructions(patchText);
                    ifMap[ifStatement] = instructions;
                }
                ifType = patchText.parseTerm().toModIfStatementType();
            }

            return new ModIfStatement(ifMap);
        }

        public static ModIfStatementClause parseIfStatementClause(this ModParser modParser,
                                                                  ModIfStatementType ifType,
                                                                  ParsingContainer patchText)
        {
            if (ifType == ModIfStatementType.Else) {
                return new ModIfStatementClause(ifType, null);
            } else {
                ModBooleanExpression boolExpression = modParser.parseBooleanExpression(patchText);
                return new ModIfStatementClause(ifType, boolExpression);
            }
        }
    }
}
