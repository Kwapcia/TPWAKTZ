using Data;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Logic
{
    public class Logic : LogicApi
    {
        private Vector2 size = new Vector2(750, 750);
        private DataApi data;

        public Logic()
        {
            Vector2 cords = PutBallOnBoard();
            data = DataApi.CreateBall(cords.X, cords.Y);
        }

        public override DataApi GetDataAPI()
        {
            return data;
        }


        public override Vector2 getBallPosition()
        {
            return new Vector2((float)data.getXPosition(), (float)data.getYPosition());
        }

        public override void setBallXPosition(double x)
        {
            data.setXPosition(x);
        }

        public override void setBallYPosition(double y)
        {
            data.setYPosition(y);
        }

        public override Vector2 PutBallOnBoard()
        {
            Random r = new Random();
            double x = r.NextDouble() * size.X;
            r = new Random();
            double y = r.NextDouble() * size.Y;
            y += 30;
            return new Vector2((float)x, (float)y);
        }

        public override Vector2 NextStepPosition(Vector2 position, Vector2 nextPosition, int steps)
        {
            Vector2 movement = nextPosition - position;
            return position + (movement / steps);
        }

        public override LogicApi CreateBall()
        {
            Vector2 coords = PutBallOnBoard();
            data = DataApi.CreateBall(coords.X, coords.Y);
            return LogicApi.CreateObjLogic(data);
        }
    }
}