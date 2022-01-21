using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Exceptions {
    class FileNotOpenedException: Exception {
        public FileNotOpenedException(String filePath) : base(String.Format("{0} could not be opened", filePath)) { }
    }
}
