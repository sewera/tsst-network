using Common.Utils;
using NUnit.Framework;

namespace Common.test.Utils
{
    [TestFixture]
    public class CheckersTest
    {
        [Test]
        public void PortMatchesTest_ReturnTrue()
        {
            const string pattern = "3xx";
            string[] ports = {"321", "384", "300"};
            foreach (string port in ports) Assert.IsTrue(Checkers.PortMatches(pattern, port));
        }

        [Test]
        public void PortMatchesTest_ReturnFalse()
        {
            const string pattern = "3xx";
            string[] ports = {"123", "231", "3a1", "3121", "31"};
            foreach (string port in ports) Assert.IsFalse(Checkers.PortMatches(pattern, port));
        }
        
        [Test]
        public void MultipleGatewaysInRibRow_ReturnTrue()
        {
            string[] gateways = {"123,222", "231,000", "311,212"};
            foreach (string gateway in gateways) Assert.IsTrue(Checkers.MultipleGatewaysInRibRow(gateway));
        }
        
        [Test]
        public void MultipleGatewaysInRibRow_ReturnFalse()
        {
            string[] gateways = {"123", "0", "312213"};
            foreach (string gateway in gateways) Assert.IsFalse(Checkers.MultipleGatewaysInRibRow(gateway));
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
                ((8, 12), (5, 10)),
                ((10, 12), (5, 15)),
                ((5, 15), (10, 12)),
                ((1, 1), (1, 2)),
                ((1, 2), (1, 1)),
                ((1, 1), (1, 1)),
                ((0, 1), (0, 99)),
            }; 

            foreach (((int, int) slot1, (int, int) slot2) in slotPairs)
            {
                Assert.IsTrue(Checkers.SlotsOverlap(slot1, slot2));
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
                Assert.IsFalse(Checkers.SlotsOverlap(slot1, slot2));
            }
        }
    }
}
