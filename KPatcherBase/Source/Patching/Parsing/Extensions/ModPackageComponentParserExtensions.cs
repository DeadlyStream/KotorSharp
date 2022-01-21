using AuroraIO;
using KPatcher.Patching.Models;
using KPatcherBase.Source.Models.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public static class ModPackageComponentParserExtensions { 

        public static ModPackageComponent parsePackageComponent(this ModParser modParser,
                                                                ParsingContainer textContainer)
        {
            String packageWord = textContainer.parseTerm();
            switch (packageWord) {
                case ReservedWord.General.Opt:
                    return parseOption(textContainer);
                case ReservedWord.General.Var:
                    return modParser.parseVar(textContainer);
                case ReservedWord.General.SetVar:
                    return modParser.parseSetVar(textContainer);
                case ReservedWord.General.If:
                    return modParser.parseIf(textContainer);
                case ReservedWord.Package.Install:
                    return modParser.parseInstall(textContainer);
                case ReservedWord.Package.Patch:
                    return modParser.parsePatch(textContainer);
                default:
                    throw textContainer.throwException(packageWord, "package instruction");
            }
        }

        public static ModFilePatchSet parsePatch(this ModParser modParser, ParsingContainer patchText) {
            String fileName = patchText.parseTerm();
            AuroraResourceType fileType = patchText.parseTerm().toAuroraResourceType();
            List<ModFilePatchCommand> commands = new List<ModFilePatchCommand >();
            patchText.parseColon();
            while (!patchText.tryParseEndMarker()) {
                commands.Add(modParser.parseFilePatchCommand(patchText, fileType));
            }
            return new ModFilePatchSet(fileName, fileType, commands.ToArray());
        }

        public static ModVar parseVar(this ModParser modParser, ParsingContainer patchText) {
            String varName = patchText.parseTerm();
            ModValueExpression initialValue = null;
            if (patchText.tryParseEqualAssignment()) {
                initialValue = modParser.parseValueExpression(patchText);
            }
            return new ModVar(varName, initialValue);
        }

        public static ModVar parseSetVar(this ModParser modParser, ParsingContainer patchText) {
            String varName = patchText.parseTerm();
            patchText.tryParseEqualAssignment();
            ModValueExpression initialValue = modParser.parseValueExpression(patchText);
            return new ModVar(varName, initialValue);
        }

        public static ModOption parseOption(ParsingContainer patchText) {
            String optionName = patchText.parseTerm();
            patchText.parseColon();
            List<String> options = new List<string>();
            while (!patchText.tryParseEndMarker()) {
                options.Add(patchText.parseTerm());
            }
            return new ModOption(optionName, options);
        }
    }
}
