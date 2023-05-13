using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTest
{
    [TestClass]
    public class CollisionTest
    {
        private LogicAbstractApi Api;

        [TestMethod]
        public void testCollision()
        {
            Api = LogicAbstractApi.createApi(800, 600);
            Api.createBalls(2);

            Api.getBall(0).ballNewX = 5;
            Api.getBall(0).ballNewY = 5;
            Api.getBall(1).ballNewX = -3;
            Api.getBall(1).ballNewY = -3;

            Api.getBall(0).ballX = 20;
            Api.getBall(1).ballX = 30;
            Api.getBall(0).ballY = 20;
            Api.getBall(1).ballY = 30;
            Api.getBall(0).ballMove();
            Api.getBall(1).ballMove();

            Assert.AreNotEqual(-3, Api.getBall(1).ballNewX);
            Assert.AreNotEqual(-3, Api.getBall(1).ballNewY);
            Assert.AreNotEqual(5, Api.getBall(0).ballNewX);
            Assert.AreNotEqual(5, Api.getBall(0).ballNewY);
        }

        [TestMethod]
        public void testWallCollision()
        {
            Api = LogicAbstractApi.createApi(800, 600);
            Api.createBalls(1);
            Api.getBall(0).ballNewX = 5;
            Api.getBall(0).ballX = 790;
            Assert.AreNotEqual(5, Api.getBall(0).ballNewX);
            Api.getBall(0).ballNewX = -3;
            Api.getBall(0).ballX = -3;
            Assert.AreNotEqual(-3, Api.getBall(0).ballNewX);
            Api.getBall(0).ballNewY = -7;
            Api.getBall(0).ballY = -2;
            Assert.AreNotEqual(-7, Api.getBall(0).ballNewY);
            Api.getBall(0).ballNewY = 7;
            Api.getBall(0).ballY = 607;
            Assert.AreNotEqual(7, Api.getBall(0).ballNewY);
        }
    }
}