using KPatcher.Patching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public class ParsingContainer {
        private int currentLine = 1;
        private string patchText;

        public ParsingContainer(String patchText) {
            this.patchText = patchText;
            trimSeparatorCharacters();
        }

        public String peekTerm() {
            if (patchText == null) {
                return null;
            }
            String termRegex = "[\\w\\S]+(?<!:)";
            String quotedRegex = "\"[^\"]*\"";
            String term = Regex.Match(patchText, String.Format("({0})|({1})", quotedRegex, termRegex)).Value;
            return term;
        }

        public int parseInt() {
            String term = parseTerm();
            try {
                return Int32.Parse(term);
            } catch {
                throw new Exception(String.Format("Line: {0} Expected int type", currentLine));
            }
        }

        public double parseDouble() {
            String term = parseTerm();
            try {
                return Double.Parse(term);
            } catch {
                throw new Exception(String.Format("Line: {0} Expected double type", currentLine));
            }
        }

        public String parseHexString() {
            String term = parseTerm();
            String hexString = Regex.Match(term, "([A-Fa-f0-9]{2})*").Value;
            if (hexString.Equals(term)) {
                return hexString;
            } else {
                throw new Exception(String.Format("Line: {0} Expected hex string type", currentLine));
            }
        }

        public String parseTerm() {
            String peekedTerm = peekTerm();
            int matchCount = Regex.Matches(peekedTerm, "\\n").Count;
            currentLine += matchCount;
            patchText = patchText.Remove(0, peekedTerm.Length);
            trimSeparatorCharacters();
            return peekedTerm;
        }

        public String parseExpectedTerm(String expectedTerm) {
            String peekedTerm = peekTerm();
            if (peekedTerm == expectedTerm) {
                int matchCount = Regex.Matches(peekedTerm, "\\n").Count;
                currentLine += matchCount;
                patchText = patchText.Remove(0, peekedTerm.Length);
                trimSeparatorCharacters();
                return peekedTerm;
            } else {
                throw new Exception(String.Format("Line:{0} Expected term `{1}`", currentLine, expectedTerm));
            }
        }

        public void parseColon() {
            trimSeparatorCharacters();
            if (patchText.isNextCharColon()) {
                patchText = patchText.removeFirstChar();
                trimSeparatorCharacters();
            } else {
                throw new Exception(String.Format("Line:{0} Expected ':'", currentLine));
            }
        }

        public bool tryParseEndMarker() {
            if (peekTerm() == ReservedWord.General.End) {
                parseExpectedTerm(ReservedWord.General.End);
                return true;
            } else {
                return false;
            }
        }

        public bool tryParseEndIfMarker() {
            String peekedTerm = peekTerm();

            switch (peekedTerm) {
                case ReservedWord.General.ElseIf:
                case ReservedWord.General.Else:
                case ReservedWord.General.EndIf:
                    return true;
                default: return false;
            }
        }

        public bool tryParseEqualAssignment() {
            trimSeparatorCharacters();
            String term = Regex.Match(patchText, "^={1}").Value;
            if (term == "=") {
                patchText = patchText.Remove(0, 1);
                trimSeparatorCharacters();
                return true;
            } else {
                return false;
            }
        }

        public bool hasBooleanComparer() {
            return patchText.StartsWith("==") || patchText.StartsWith("!=");
        }

        public string parseBooleanComparerString() {
            String match = Regex.Match(patchText, "^(!=)|(==)").Value;
            patchText = patchText.Remove(0, match.Length);
            trimSeparatorCharacters();
            return match;
        }

        public void parseEndMarker() {
            parseExpectedTerm(ReservedWord.General.End);
        }

        private void trimSeparatorCharacters() {
            String whitespaceRegex = "[\\r\\n\\s\\t]+";
            String singleLineCommentRegex = "\\/\\/.*\\n";
            String multiLineCommentRegex = "\\/\\*(([\\s\\S\\w](?<!\\*\\/))*\\*\\/)+";
            String fullRegex = String.Format("^(({0})|({1})|({2}))+", whitespaceRegex, singleLineCommentRegex, multiLineCommentRegex);
            Match match = Regex.Match(patchText, fullRegex);
            String matchString = match.Value;
            int matchCount = Regex.Matches(matchString, "\\n").Count;
            currentLine += matchCount;
            patchText = patchText.Remove(0, match.Length);
        }

        public override string ToString() {
            return patchText;
        }

        public Exception throwException(String badTerm, String termType) {
            return new Exception(String.Format("Line: {0} Unrecognized {1} `{2}`", currentLine, termType, badTerm));
        }
    }

}
