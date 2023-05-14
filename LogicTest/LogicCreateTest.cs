using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTest
{

    [TestClass]
    public class CreationTest
    {
        private LogicAbstractApi Api;

        [TestMethod]
        public void testCreateApi()
        {
            Api = LogicAbstractApi.createApi(800, 600);
            Assert.IsNotNull(Api);
        }

        [TestMethod]
        public void getCount()
        {
            Api = LogicAbstractApi.createApi(800, 600);
            Assert.AreEqual(800, Api.width);
            Assert.AreEqual(600, Api.height);
            Api.createBalls(5);
            Assert.AreEqual(5, Api.getAmount);
            Api.createBalls(-3);
            Assert.AreEqual(2, Api.getAmount);
            Api.createBalls(-3);
            Assert.AreEqual(0, Api.getAmount);
        }
    }
}
