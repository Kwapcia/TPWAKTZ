using Data;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Logic
{
    // Klasa Logic implementująca interfejs LogicApi
    public class Logic : LogicApi
    {
        // Wartość rozmiaru planszy
        private Vector2 size = new Vector2(750, 750);
        // Obiekt DataApi przechowujący informacje o piłce
        private DataApi data;

        // Konstruktor klasy Logic
        public Logic()
        {
            // Ustawia piłkę na losowej pozycji na planszy
            Vector2 cords = PutBallOnBoard();
            // Tworzy nowy obiekt DataApi dla piłki
            data = DataApi.CreateBall(cords.X, cords.Y);
        }

        // Metoda zwracająca obiekt DataApi
        public override DataApi GetDataAPI()
        {
            return data;
        }

        // Metoda zwracająca pozycję piłki jako wektor Vector2
        public override Vector2 getBallPosition()
        {
            return new Vector2((float)data.getXPosition(), (float)data.getYPosition());
        }

        // Metoda ustawiająca pozycję piłki na osi X
        public override void setBallXPosition(double x)
        {
            data.setXPosition(x);
        }

        // Metoda ustawiająca pozycję piłki na osi Y
        public override void setBallYPosition(double y)
        {
            data.setYPosition(y);
        }

        // Metoda ustawiająca piłkę na losowej pozycji na planszy
        public override Vector2 PutBallOnBoard()
        {
            Random r = new Random();
            double x = r.NextDouble() * size.X;
            r = new Random();
            double y = r.NextDouble() * size.Y;
            y += 30;
            return new Vector2((float)x, (float)y);
        }

        // Metoda zwracająca pozycję piłki po wykonaniu kroku na planszy
        public override Vector2 NextStepPosition(Vector2 position, Vector2 nextPosition, int steps)
        {
            Vector2 movement = nextPosition - position;
            return position + (movement / steps);
        }

        // Metoda tworząca nowy obiekt LogicApi (piłkę)
        public override LogicApi CreateBall()
        {
            Vector2 coords = PutBallOnBoard();
            data = DataApi.CreateBall(coords.X, coords.Y);
            return LogicApi.CreateObjLogic(data);
        }

    }
}