using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Base {
    public abstract class AuroraResourceCollection : AuroraResource {

        public abstract void extractAll(String filePath);

        public override byte[] toBytes() {
            throw new NotImplementedException();
        }
    }
}
