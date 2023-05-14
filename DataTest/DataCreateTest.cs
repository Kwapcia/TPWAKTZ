using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestData
{

    [TestClass]
    public class DataCreationTest
    {
        private DataAbstractApi Api;

        [TestMethod]
        public void testCreateApi()
        {
            Api = DataAbstractApi.createApi(800, 600);
            Assert.IsNotNull(Api);
        }

        [TestMethod]
        public void getCount()
        {
            Api = DataAbstractApi.createApi(800, 600);
            Assert.AreEqual(800, Api.width);
            Assert.AreEqual(600, Api.height);
            Api.createBallsList(5);
            Assert.AreEqual(5, Api.getAmount);
            Api.createBallsList(-3);
            Assert.AreEqual(2, Api.getAmount);
            Api.createBallsList(-3);
            Assert.AreEqual(0, Api.getAmount);
        }

        [TestMethod]
        public void getBall()
        {
            Api = DataAbstractApi.createApi(800, 600);
            Api.createBallsList(3);
            Assert.AreNotEqual(Api.getBall(0), Api.getBall(1));
            Assert.AreNotEqual(Api.getBall(1), Api.getBall(2));
            Assert.AreNotEqual(Api.getBall(0), Api.getBall(2));
        }
    }
}