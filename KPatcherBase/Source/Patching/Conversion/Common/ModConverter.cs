using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Modicron.Source.Operations {
    public class ModConverter {
        public static void analyzeDirectory(String directory) {
            String[] tslPatcherDirectories = Directory.GetDirectories(directory, "tslpatchdata", SearchOption.AllDirectories);

            if (tslPatcherDirectories.Length > 0) {
                //Do TSLPatcher conversion
            } else {
                String unzipDirectory = directory + "/unzip";
                DirectoryTree tree = new DirectoryTree(unzipDirectory);
                int depth = tree.depth;
                DirectoryTree[] leaves = tree.leafNodes;

                Console.WriteLine(String.Format("[{0}]", directory));

                if (leaves.Length <= 0) {
                    Console.WriteLine("Not supported");
                } else if (leaves.Length <= 1) {
                    Console.WriteLine("(One option, easy to install)");
                    DirectoryTree leaf = leaves.First();
                    Console.WriteLine(String.Format("- {0}", leaf.directoryNodeName));
                } else {
                    Console.WriteLine("(More complex, need to decide)");
                    foreach (DirectoryTree leaf in leaves) {
                        Console.WriteLine(String.Format("- {0} = {1}", leaf.directoryNodeName, leaf.designation));
                    }

                    int overrides = leaves.Where(leaf => leaf.designation == DirectoryDesignation.Override).Count();
                    int kotorDirectories = leaves.Where(leaf => leaf.designation == DirectoryDesignation.KotorDirectory).Count();

                    Console.WriteLine("Overrides: " + overrides);
                    Console.WriteLine("Kotor Directories: " + kotorDirectories);
                }
            }           
        }
    }
}
