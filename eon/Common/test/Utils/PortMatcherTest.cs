using Common.Utils;
using NUnit.Framework;

namespace Common.test.Utils
{
    [TestFixture]
    public class PortMatcherTest
    {
        [Test]
        public void PortMatchesTest_ReturnTrue()
        {
            const string pattern = "3xx";
            string[] ports = {"321", "384", "300"};
            foreach (string port in ports) Assert.IsTrue(ICheckers.PortMatches(pattern, port));
        }

        [Test]
        public void PortMatchesTest_ReturnFalse()
        {
            const string pattern = "3xx";
            string[] ports = {"123", "231", "3a1", "3121", "31"};
            foreach (string port in ports) Assert.IsFalse(ICheckers.PortMatches(pattern, port));
        }

        [Test]
        public void SlotsOverlapTest_ReturnTrue()
        {
            ((int, int), (int, int))[] slotPairs =
            {
                ((8, 10), (8, 10)),
                ((5, 10), (8, 12)),
                ((8, 10), (10, 12)),
                ((10, 12), (8, 10)),
                ((8, 12), (5, 10))
            };

            foreach (((int, int) slot1, (int, int) slot2) in slotPairs)
            {
                Assert.IsTrue(ICheckers.SlotsOverlap(slot1, slot2));
            }
        }

        [Test]
        public void SlotsOverlapTest_ReturnFalse()
        {
            ((int, int), (int, int))[] slotPairs =
            {
                ((8, 10), (11, 20)),
                ((5, 10), (15, 32)),
                ((8, 8), (9, 9))
            };

            foreach (((int, int) slot1, (int, int) slot2) in slotPairs)
            {
                Assert.IsFalse(ICheckers.SlotsOverlap(slot1, slot2));
            }
        }
    }
}
