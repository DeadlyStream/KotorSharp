using AuroraIO.Source.Common;
using AuroraIO.Source.Models._2da;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AuroraIOTests.Source.Asserts
{
    public static class AIOAssert
    {
        public static void VerifyFile(_2DAObject actual, MethodBase methodBase, bool record)
        {
            var asciiCoder = new ASCIICoder();
            var _2daCoder = new _2DACoder();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artifacts", String.Format("{0}_{1}", methodBase.DeclaringType.Name, methodBase.Name));

            if (record) {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                File.WriteAllBytes(path, _2daCoder.encode(actual));
            }
            var expected = _2daCoder.decode(File.ReadAllBytes(path));
            Assert.AreEqual(asciiCoder.encode(expected),
                          asciiCoder.encode(actual));
        }
    }
}
