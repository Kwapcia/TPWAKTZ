using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System.Numerics;

namespace LogicTest
{
    [TestClass]
    public class LogicApiTest
    {
        LogicApi testBall;
        Vector2 position;
        Vector2 nextPosition;
        Vector2 ballSteps;
        [TestMethod]
        public void TestBallMovement()
        {
            testBall = LogicApi.CreateObjLogic();
            position = new Vector2(5, 30);
            nextPosition = new Vector2(30, 5);
            int steps = 5;
            ballSteps = testBall.NextStepPosition(position, nextPosition, steps);
            Assert.AreEqual(ballSteps.X, ((nextPosition.X - position.X) / steps) + position.X);
            Assert.AreEqual(ballSteps.Y, ((nextPosition.Y - position.Y) / steps) + position.Y);
        }
    }
}}
