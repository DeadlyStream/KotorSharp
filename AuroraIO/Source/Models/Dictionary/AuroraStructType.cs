using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public interface AuroraStructType: IEnumerable<KeyValuePair<string, AuroraDataObject>> {
        uint structType { get; }
    }
}
