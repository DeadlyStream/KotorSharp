using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Patcher {
    public enum LogLevel {
        none, info, error, warn, debug
        //0) No feedback at all. The text from "info.rtf" will continue to be displayed during installation.
        //1) Only general progress information will be displayed. Not recommended.
        //2) General progress information is displayed, along with any serious errors encountered.
        //3) General progress information, serious errors and warnings are displayed. This is recommended for the release version of your mod.
        //4) Full feedback. On top of what is displayed at level 3, it also shows verbose progress information that may be useful for a Modder to see what is happening. Intended for Debugging.

    }
}
