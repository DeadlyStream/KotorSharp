using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Patcher {
    public struct NamespaceCollection {
        Dictionary<string, NamespaceInfo> namespaces;

        internal NamespaceCollection(Dictionary<String, NamespaceInfo> namespaces) {
            this.namespaces = namespaces;
        }

        public NamespaceInfo this[String key] {
            get {
                return namespaces[key];
            }
        }
    }
}
