using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives {

    public abstract class AuroraArchiveEntry {
        public static AuroraArchiveEntry make<T>() where T: AuroraArchiveEntry {
            return default(T);
        }
    }

    public abstract class AuroraArchive<Key, Value> where Value : AuroraArchiveEntry {

        private Dictionary<Key, Value> internalMap;
        public Value this[Key key] { 
            get {
                return internalMap[key];
            } set {
                internalMap[key] = value;
            }
        }
    }
}
