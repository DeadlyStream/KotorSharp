using KPatcher.Patching.Models;
using KPatcher.Patching.Models.InstallCommands;
using KPatcherBase.Source.Models.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public static class ModFileInstallCommandParsingExtensions {
        public static ModFileInstallSet parseInstall(this ModParser modParser, ParsingContainer textContainer) {
            String installDirectory = textContainer.parseTerm();
            textContainer.parseColon();

            List<ModFileInstallCommand> commands = new List<ModFileInstallCommand>();

            String word = textContainer.parseTerm();
            while (word != ReservedWord.General.End) {
                String fileName;
                switch (word) {
                    case ReservedWord.Install.Copy:
                        fileName = textContainer.parseTerm();
                        commands.Add(new SingleFileCopyInstallCommand(fileName, fileName));
                        break;
                    case ReservedWord.Install.CopyRename:
                        fileName = textContainer.parseTerm();
                        String newFileName = textContainer.parseTerm();
                        commands.Add(new SingleFileCopyInstallCommand(fileName, newFileName));
                        break;
                    case ReservedWord.Install.CopyFolder:
                        String folderName = textContainer.parseTerm();
                        commands.Add(new FolderCopyInstallCommand(folderName));
                        break;
                    case ReservedWord.General.Var:
                        commands.Add(modParser.parseVar(textContainer));
                        break;
                    case ReservedWord.General.SetVar:
                        commands.Add(modParser.parseSetVar(textContainer));
                        break;
                    case ReservedWord.General.If:
                        commands.Add(modParser.parseIf(textContainer));
                        break;
                    default:
                        throw textContainer.throwException(word, "install command");
                }
                word = textContainer.parseTerm();
            }
            return new ModFileInstallSet(installDirectory, commands.ToArray());
        }
    }
}
