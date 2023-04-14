using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using System.Numerics;

namespace Logic
{
    public abstract class LogicApi
    {
        public abstract LogicApi CreateBall();
        public abstract Vector2 PutBallOnBoard();
        public abstract Vector2 getBallPosition();
        public abstract void setBallXPosition(double XPos);
        public abstract void setBallYPosition(double YPos);
        public abstract Vector2 NextStepPosition(Vector2 currentPos, Vector2 targetPos, int stepCount);
        public abstract DataApi GetDataAPI();

        public static LogicApi CreateObjLogic(DataApi data = default(DataApi))
        {
            return new Logic();
        }
    }
}
