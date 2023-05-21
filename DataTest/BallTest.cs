using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        private DataAbstractApi Api;

        [TestMethod]
        public void moveTest()
        {
            Api = DataAbstractApi.createApi(800, 600);
            Api.createBallsList(1);
            double x = Api.getBall(0).BallPosition.X;
            double y = Api.getBall(0).BallPosition.Y;
            Api.getBall(0).BallNewPosition = new Vector2(5, 5);
            Api.getBall(0).ballMove();
            Assert.AreNotEqual(x, Api.getBall(0).BallPosition.X);
            Assert.AreNotEqual(y, Api.getBall(0).BallPosition.Y);
        }

        [TestMethod]
        public void setTests()
        {
            Api = DataAbstractApi.createApi(800, 600);
            Api.createBallsList(1);
            Api.getBall(0).BallPosition = new Vector2(10, 17);
            Api.getBall(0).BallNewPosition = new Vector2(4, -3);
            Assert.AreEqual(10, Api.getBall(0).BallPosition.X);
            Assert.AreEqual(17, Api.getBall(0).BallPosition.Y);
            Assert.AreEqual(4, Api.getBall(0).BallNewPosition.X);
            Assert.AreEqual(-3, Api.getBall(0).BallNewPosition.Y);
        }

        [TestMethod]
        public void velocityTest()
        {
            Api = DataAbstractApi.createApi(800, 600);
            Api.createBallsList(1);
            Api.getBall(0).Velocity = new Vector2(2, 3);
            Assert.AreEqual(2, Api.getBall(0).Velocity.X);
            Assert.AreEqual(3, Api.getBall(0).Velocity.Y);
        }
    }
}
