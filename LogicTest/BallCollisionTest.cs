using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

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

            Api.getBall(0).BallPosition = new Vector2(20, 20);
            Api.getBall(1).BallPosition = new Vector2(30, 30);
            Api.getBall(0).ballMove();
            Api.getBall(1).ballMove();

            Assert.AreNotEqual(-3, Api.getBall(1).BallPosition.X);
            Assert.AreNotEqual(-3, Api.getBall(1).BallPosition.Y);
            Assert.AreNotEqual(5, Api.getBall(0).BallPosition.X);
            Assert.AreNotEqual(5, Api.getBall(0).BallPosition.Y);
        }

        [TestMethod]
        public void testWallCollision()
        {
            Api = LogicAbstractApi.createApi(800, 600);
            Api.createBalls(1);
            Api.getBall(0).BallPosition = new Vector2(790, 0);
            Api.getBall(0).ballMove();
            Assert.AreNotEqual(5, Api.getBall(0).BallPosition.X);

            Api.getBall(0).BallPosition = new Vector2(-3, 0);
            Api.getBall(0).ballMove();
            Assert.AreNotEqual(-3, Api.getBall(0).BallPosition.X);

            Api.getBall(0).BallPosition = new Vector2(0, -2);
            Api.getBall(0).ballMove();
            Assert.AreNotEqual(-7, Api.getBall(0).BallPosition.Y);

            Api.getBall(0).BallPosition = new Vector2(0, 607);
            Api.getBall(0).ballMove();
            Assert.AreNotEqual(7, Api.getBall(0).BallPosition.Y);
        }

    }
}
