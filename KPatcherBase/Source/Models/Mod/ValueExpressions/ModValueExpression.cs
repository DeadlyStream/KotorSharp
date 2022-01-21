using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models {
    public interface ModValueExpression {
        string evaluatedValue();
        void store(string value);
    }
}