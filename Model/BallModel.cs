using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BallModel
    {
        private LogicApi logic;
        private double X;
        private double Y;

        public BallModel()
        {
            logic = LogicApi.CreateObjLogic();
            X = logic.getBallPosition().X;
            Y = logic.getBallPosition().Y;
        }

        // zwraca i ustawia wartosc pola X na osi
        public double ModelXPosition
        {
            get
            {
                return logic.getBallPosition().X;
            }
            set
            {
                logic.setBallXPosition(value);
            }
        }

        // zwraca i ustawia wartosc pola Y na osi
        public double ModelYPosition
        {
            get
            {
                return logic.getBallPosition().Y;
            }
            set
            {
                logic.setBallYPosition(value);
            }
        }


        // zwraca aktualne polozenie pilki
        // zwraca wektor 2 wymiarowy o dwoch polach float
        public Vector2 getModelPosition()
        {
            return new Vector2((float)ModelXPosition, (float)ModelYPosition);
        }

        // używa metody PutBallOnBoard() z biblioteki LogicApi, aby ustawić pilke
        // na planszy i zwrócić jej aktualne położenie jako wektor Vector2
        public Vector2 GetBallPosition()
        {
            return logic.PutBallOnBoard();
        }

    }
}

