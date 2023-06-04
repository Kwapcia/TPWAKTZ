using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using System.Numerics;

namespace TestData
{


    [TestClass]
    public class BallTest
    {
        private DataAbstractApi DApi;

        [TestMethod]
        public void createIBallTest()
        {
            DApi = DataAbstractApi.createApi(800, 600);
            IBall b = DApi.createBall(1);
            Assert.AreEqual(1, b.ballID);

            Assert.IsTrue(b.ballPosition.X >= b.ballSize);
            Assert.IsTrue(b.ballPosition.X <= (DApi.width - b.ballSize));
            Assert.IsTrue(b.ballPosition.Y >= b.ballSize);
            Assert.IsTrue(b.ballPosition.Y <= (DApi.height - b.ballSize));

            Assert.AreEqual(30, b.ballSize);
            Assert.IsTrue(b.ballWeight == b.ballSize);
            Assert.IsTrue(b.ballVelocity.X >= -5 && b.ballVelocity.X <= 6);
            Assert.IsTrue(b.ballVelocity.Y >= -5 && b.ballVelocity.Y <= 6);
        }

        [TestMethod]
        public void moveTest()
        {
            DApi = DataAbstractApi.createApi(800, 600);
            IBall b = DApi.createBall(1);
            double x = b.ballPosition.X;
            double y = b.ballPosition.Y;
            b.ballChangeSpeed(new Vector2(5, 5));
            ConcurrentQueue<IBall> queue = new ConcurrentQueue<IBall>();
            b.moveBall(1, queue);
            Assert.AreNotEqual(x, b.ballPosition.X);
            Assert.AreNotEqual(y, b.ballPosition.Y);
            ;
        }




    }
}