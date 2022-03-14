using AuroraIO.Source.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public interface AuroraStructType: IEnumerable<KeyValuePair<CResRef, AuroraDataObject>>, SetKeyValueInterface {
        uint structType { get; }

        AuroraDataObject this[String key] { get; set; }
    }
}
