using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KPatcher.Source.Ini {
    public class IniParser {

        public static class ParsingRegex {
            public const String SectionHeader = "\\[\\w*\\]";
        }
        public IniObject parse(string filePath) {
            IniObject ini = new IniObject();
            string iniString = File.ReadAllText(filePath);

            var regex = new Regex(@"((?<=\[)[^=\r\n]*(?=\]))|(\!?\w+=[^\r\n]+)");

            IniSection currentSection = new IniSection();
            foreach(Match term in regex.Matches(iniString)) {
                if (term.Value.Contains("=")) {
                    string[] components = term.Value.Split("=");
                    currentSection[components[0]] = components[1];
                } else {
                    currentSection = new IniSection();
                    ini[term.Value] = currentSection;
                }
            }

            return ini;
        }
    }
}
