using KPatcher.Patching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    static class ModTermParserExtensions {
        public static ModBooleanOperator parseBooleanOperator(ParsingContainer patchText) {
            if (patchText.hasBooleanComparer()) {
                String booleanComparerString = patchText.parseBooleanComparerString();
                ModBooleanOperator booleanComparer = booleanComparerString.toBooleanComparer();
                return booleanComparer;
            } else {
                throw new Exception("Not a recognizable boolean operator!");
            }
        }

        public static ModValueExpression parseValueExpression(this ModParser modParser,
                                                              ParsingContainer patchText)
        {
            String firstTerm = patchText.peekTerm();
            if (firstTerm == ReservedWord.General.Where) {
                patchText.parseExpectedTerm(ReservedWord.General.Where);
                ModBooleanExpression booleanExpression = modParser.parseBooleanExpression(patchText);
                return new ModWhereExpression(booleanExpression);
            } else {
                patchText.parseTerm();
                return new ModSimpleExpression(firstTerm);
            }
        }

        public static ModBooleanExpression parseBooleanExpression(this ModParser modParser,
                                                                  ParsingContainer patchText)
        {
            String lhs = patchText.parseTerm();
            String rhs = null;
            ModBooleanOperator booleanOperator = ModBooleanOperator.Invalid;
            if (patchText.hasBooleanComparer()) {
                booleanOperator = parseBooleanOperator(patchText);
                rhs = patchText.parseTerm();
            }
            return new ModBooleanExpression(lhs, rhs, booleanOperator);
        }
    }
}
