using Common.Utils;
using NUnit.Framework;

namespace Common.test.Utils
{
    [TestFixture]
    public class CheckersTest
    {
        [Test]
        public void PortMatchesTest_ValidPorts()
        {
            (string, string, int)[] patternsPortsAndMatches =
            {
                ("xxx", "123", 0),
                ("1xx", "123", 1),
                ("12x", "123", 2),
                ("123", "123", 3)
            };

            foreach ((string pattern, string port, int matches) in patternsPortsAndMatches)
            {
                Assert.AreEqual(Checkers.PortMatches(pattern, port), matches);
            }
        }

        [Test]
        public void PortMatchesTest_InvalidPorts_ReturnMinusOne()
        {
            (string, string)[] patternsAndPorts =
            {
                ("1xx", "212"),
                ("123", "124"),
                ("11x", "124")
            };

            foreach ((string pattern, string port) in patternsAndPorts)
            {
                Assert.AreEqual(Checkers.PortMatches(pattern, port), -1);
            }
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
