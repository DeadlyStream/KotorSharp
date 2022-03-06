using AuroraIO.Source.Models.Sound;
using AuroraIOTests.Source.Asserts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source {

    [TestClass]
    public class AuroraSoundSetTests {

        [TestMethod]
        public void testEmptySoundSet() {
            var soundSet = new AuroraSoundSet();
            Snapshot.Verify(soundSet);
        }

        [TestMethod]
        public void testSetEntries() {
            var soundSet = new AuroraSoundSet();
            soundSet[AuroraSoundSet.Entry.BattleCry1] = 0;
            soundSet[AuroraSoundSet.Entry.AttackGrunt1] = 0;
            soundSet[AuroraSoundSet.Entry.Poisoned] = 0;
            Snapshot.Verify(soundSet);
        }
    }
}
