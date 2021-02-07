using Common.Utils;
using NUnit.Framework;

namespace Common.test.Utils
{
    [TestFixture]
    public class ChoosersTest
    {
        [Test]
        public void GetGatewayFromRibRowTest_ReturnSingle()
        {
            string[] ribRowGateways =
            {
                "123",
                "456"
            };

            foreach (string rowGateway in ribRowGateways)
            {
                Assert.AreEqual(rowGateway, Choosers.GetGatewayFromRibRow(rowGateway));
            }
        }

        [Test]
        public void GetGatewayFromRibRowTest_ReturnRandom()
        {
            string[] ribRowGateways =
            {
                "456,123",
                "123,456"
            };

            foreach (string rowGateway in ribRowGateways)
            {
                var gateway = Choosers.GetGatewayFromRibRow(rowGateway);
                Assert.IsTrue(gateway == "123" || gateway == "456");
            }
        }
    }
}
