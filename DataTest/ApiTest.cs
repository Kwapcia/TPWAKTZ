using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;

namespace DataTest
{
    [TestClass]
    public class DataAPITest
    {
        DataApi testBall;
        [TestMethod]
        public void BallTest()
        {
            testBall = DataApi.CreateBall(5, 7);
            Assert.AreEqual(testBall.getXPosition(), 5);
            Assert.AreEqual(testBall.getYPosition(), 7);
        }

        [TestMethod]
        public void TestBallSetValues()
        {
            testBall = DataApi.CreateBall(5, 7);
            Assert.AreEqual(testBall.getXPosition(), 5);
            Assert.AreEqual(testBall.getYPosition(), 7);
            testBall.setXPosition(6);
            testBall.setYPosition(2);
            Assert.AreEqual(testBall.getXPosition(), 6);
            Assert.AreEqual(testBall.getYPosition(), 2);
        }
    }
}