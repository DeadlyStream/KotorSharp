using AuroraIO;
using KotorManifest.Source.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Modicron.Source.Operations {
    public class DirectoryTree {
        public String directoryNodeName;
        public String parent;
        public Dictionary<String, DirectoryTree> map = new Dictionary<string, DirectoryTree>();
        public int fileCount;

        public int complexity {
            get {
                return leafNodes.Count();
            }
        }

        public DirectoryTree[] leafNodes {
            get {
                List<DirectoryTree> leafList = new List<DirectoryTree>();
                if (map.Values.Count > 0) {
                    leafList.AddRange(
                        map.Values
                        .SelectMany((value) => { return value.leafNodes; })
                        .ToArray()
                        );
                }
                if (fileCount > 0 && containsAuroraFiles) {
                    leafList.Add(this);
                }

                return leafList.ToArray();
            }
        }

        public int depth {
            get {
                if (map.Values.Count > 0) {
                    return 1 + map.Values.Max(value => value.depth);
                } else {
                    return 1;
                }
            }
        }

        public DirectoryTree(String directory) {
            directoryNodeName = directory;//Path.GetFileName(Path.GetDirectoryName(directory + "/new"));
            foreach(String subDirectory in Directory.GetDirectories(directory)) {
                DirectoryTree node = new DirectoryTree(subDirectory);
                map[node.directoryNodeName] = node;
            }

            fileCount = Directory.GetFiles(directory).Length;
        }

        public void flatten() {
            if (map.Count == 1) {
                DirectoryTree node = map.Values.First();
                directoryNodeName += "." + node.directoryNodeName;
                this.map = node.map;
                this.fileCount = node.fileCount;
            } else {
                foreach(DirectoryTree node in map.Values) {
                    node.flatten();
                }
            }
        }

        public override string ToString() {
            return String.Format("{0}... ({1}) file(s)", directoryNodeName, fileCount);
        }

        bool containsAuroraFiles {
            get {
                return AuroraResourceTypeExtensions.containsAuroraResources(directoryNodeName);
            }      
        }

        public DirectoryDesignation designation {
            get {
                Regex sourceRegex = new Regex("(?i)source|src");
                if (sourceRegex.Matches(directoryNodeName).Count > 0) {
                    return DirectoryDesignation.Source;
                }
                Regex backupRegex = new Regex("(?i)backup");
                if (backupRegex.Matches(directoryNodeName).Count > 0) {
                    return DirectoryDesignation.Backup;
                }
                Regex overrideRegex = new Regex("(?i)override");
                if (overrideRegex.Matches(directoryNodeName).Count > 0) {
                    return DirectoryDesignation.Override;
                }
                if (KotorConstants.Paths.isKotorDirectory(directoryNodeName)) {
                    return DirectoryDesignation.KotorDirectory;
                }
                return DirectoryDesignation.Other;
            }
        }
    }
}
