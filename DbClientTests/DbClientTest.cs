using DBManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DbClientTests
{
    [TestClass]
    public class DbClientTest
    {
        [TestMethod("Testing linked tables")]
        public void TestingLinkedTables()
        {
            var result = DBManager.DbClient.GetFinishedGoodInfo(20220705091901076);
            Assert.IsNotNull(result._FinishedGoodsDetails[0]);
            Assert.IsNotNull(result._FinishedGoodsDetails[0]._RawGoodsInfo);
        }
    }
}
