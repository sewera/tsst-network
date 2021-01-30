using System.Collections.Generic;
using NUnit.Framework;
using RoutingController.Config;

namespace RoutingController.test
{
    [TestFixture]
    public class RcStateTest
    {
        private RcState _rcState;

        [SetUp]
        public void SetUp()
        {
            List<Configuration.RouteTableRow> routeTable = new List<Configuration.RouteTableRow>
            {
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("321").SetDst("123").SetGateway("123").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("321").SetDst("12x").SetGateway("120").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("321").SetDst("1xx").SetGateway("100").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("321").SetDst("xxx").SetGateway("000,001").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("32x").SetDst("123").SetGateway("123").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("32x").SetDst("12x").SetGateway("121").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("32x").SetDst("1xx").SetGateway("101").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("32x").SetDst("xxx").SetGateway("001,002").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("3xx").SetDst("123").SetGateway("123").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("3xx").SetDst("12x").SetGateway("122").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("3xx").SetDst("1xx").SetGateway("102").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("3xx").SetDst("xxx").SetGateway("002,003").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("xxx").SetDst("123").SetGateway("123").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("xxx").SetDst("12x").SetGateway("123").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("xxx").SetDst("1xx").SetGateway("103").Build(),
                new Configuration.RouteTableRow.RouteTableRowBuilder().SetSrc("xxx").SetDst("xxx").SetGateway("003,004").Build()
            };
            _rcState = new RcState(routeTable);
        }

        [Test]
        public void GetBestGatewayTest_SingleGateway()
        {
            Assert.AreEqual("123", _rcState.GetBestGateway("321", "123")); // TODO: More tests
        }

        [Test]
        public void GetBestGatewayTest_RandomGateway()
        {
            string gateway;

            gateway = _rcState.GetBestGateway("321", "999");
            Assert.That(gateway == "000" || gateway == "001", gateway);

            gateway = _rcState.GetBestGateway("999", "999");
            Assert.That(gateway == "003" || gateway == "004", gateway); // TODO: More tests
        }
    }
}
