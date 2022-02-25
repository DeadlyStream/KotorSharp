using AuroraIO.Models.Base;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives {
    public class AuroraResourceName {
        public CResRef resref;
        public AuroraResourceType resourceType;
    }

    public class AuroraFile {
        public AuroraResourceName name;
        public byte[] data;
    }
}
