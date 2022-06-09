using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source {

    [TestClass]
    public class TestEnvironment {

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context) {
            KSEnvironment.Bundle = Bundle.current();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup() {
            Console.WriteLine("AssemblyCleanup");
        }
    }
}
