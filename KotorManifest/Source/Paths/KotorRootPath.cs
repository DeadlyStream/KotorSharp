using AppToolbox.Classes;
using KotorManifest.Source.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KotorManifest.Paths {
    public class KotorRootPath: ApplicationPath {
        public readonly ApplicationPath ChitinKey;
        public readonly ApplicationPath DialogTLK;
        public readonly ApplicationPath SWKotorINI;

        public readonly ApplicationPath Data;
        public readonly ApplicationPath Lips;
        public readonly ApplicationPath Modules;
        public readonly ApplicationPath Miles;
        public readonly ApplicationPath Movies;
        public readonly ApplicationPath Rims;
        public readonly ApplicationPath Override;
        public readonly ApplicationPath Saves;
        public readonly ApplicationPath StreamMusic;
        public readonly ApplicationPath StreamSounds;
        public readonly ApplicationPath StreamWaves;
        public readonly ApplicationPath TexturePacks;

        public readonly ApplicationPath[] mainGameDirectories;

        public KotorRootPath(string rootDirectory) : base(rootDirectory) {
            ApplicationPath root = rootDirectory;
            Data = root + KotorConstants.Paths.Data;
            Lips = root + KotorConstants.Paths.Lips;
            Modules = root + KotorConstants.Paths.Modules;
            Miles = root + KotorConstants.Paths.Miles;
            Movies = root + KotorConstants.Paths.Movies;
            Rims = root + KotorConstants.Paths.Rims;
            Override = root + KotorConstants.Paths.Override;
            Saves = root + KotorConstants.Paths.Saves;
            StreamMusic = root + KotorConstants.Paths.StreamMusic;
            StreamSounds = root + KotorConstants.Paths.StreamSounds;
            StreamWaves = root + KotorConstants.Paths.StreamWaves;
            TexturePacks = root + KotorConstants.Paths.TexturePacks;

            mainGameDirectories = new ApplicationPath[] {
                                                    Data,
                                                    Lips,
                                                    Modules,
                                                    Miles,
                                                    Movies,
                                                    Rims,
                                                    Override,
                                                    Saves,
                                                    StreamMusic,
                                                    StreamSounds,
                                                    StreamWaves,
                                                    TexturePacks
                                                };

            ChitinKey = root + "chitin.key";
            DialogTLK = root + "dialog.tlk";
            SWKotorINI = root + "swkotor.ini";
        }

        public static implicit operator KotorRootPath(String path) {
            return new KotorRootPath(path);
        }

        public static implicit operator String(KotorRootPath path) {
            return path.pathComponent;
        }

        public static ApplicationPath operator +(KotorRootPath originalPath, String newPath) {
            return originalPath.pathComponent + System.IO.Path.DirectorySeparatorChar + newPath;
        }

        public static ApplicationPath operator +(KotorRootPath originalPath, ApplicationPath newPath) {
            return originalPath.pathComponent + System.IO.Path.DirectorySeparatorChar + newPath.pathComponent;
        }
    }
}
