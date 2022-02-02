using AuroraIO.Source.Models._2da;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source.Stubs
{
    public static class _2daObjectStubs
    {
        public static _2DAObject stub1()
        {
            return new _2DAObject(
                new string[] { "column0", "column1", "column2",	"column3" },
                new string[][] {
                    new string[] { "c0r0", "c1r0", "c2r0", "c3r0" },
                    new string[] { "c0r1", "c1r1", "c2r1", "c3r1" },
                    new string[] { "c0r2", "c1r2", "c2r2", "c3r2" },
                    new string[] { "c0r3", "c1r3", "c2r3", "c3r3" }
                }
            );
        }
    }
}
