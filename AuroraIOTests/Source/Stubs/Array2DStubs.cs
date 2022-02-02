using AuroraIO.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source.Stubs
{
    public static class Array2DStubs
    {
        public static Array2D stub1() {
            Array2D test2DA = new Array2D();
            test2DA.columns = new string[] {
                "column1", "column2", "column3", "column4"
            };

            for (int i = 0; i < 4; i++)
            {
                test2DA.addRowWithValues(new string[]
                {
                    string.Format("c1r{0}", i),
                    string.Format("c2r{0}", i),
                    string.Format("c3r{0}", i),
                    string.Format("c4r{0}", i)
                });
            }
            return test2DA;
        }
    }
}
