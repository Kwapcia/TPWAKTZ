using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class BallModelCollection
    {
        List<BallModel> ballColl;
        private LogicApi logic;
        private BallsCollectionApi logicCollection;

        public void CreateBallModelCollection(int quantity)
        {
            logic = LogicApi.CreateObjLogic(); // utworzenie obiektu biblioteki LogicApi
            logicCollection = BallsCollectionApi.CreateObjCollectionLogic(); // utworzenie obiektu biblioteki BallsCollectionApi
            logicCollection.CreateBallCollection(quantity); // utworzenie kolekcji piłek o podanej ilości

            List<LogicApi> ballCollection = logicCollection.GetBallCollection(); // pobranie kolekcji piłek z BallsCollectionApi
            this.ballColl = new List<BallModel>(); // utworzenie listy modeli piłek

            foreach (LogicApi x in ballCollection) // dla każdej piłki w kolekcji piłek
            {
                BallModel ballModel = new BallModel(); // utworzenie modelu piłki
                this.ballColl.Add(ballModel); // dodanie modelu do listy
                ballModel.ModelXPosition = logic.getBallPosition().X; // ustawienie położenia X
                ballModel.ModelYPosition = logic.getBallPosition().Y; // ustawienie położenia Y
            }
        }

        public List<BallModel> GetBallModelCollection()
        {
            return ballColl;
        }

    }
}
