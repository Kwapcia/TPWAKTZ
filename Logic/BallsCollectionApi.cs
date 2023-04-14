using Data;

namespace Logic
{
    public abstract class BallsCollectionApi
    {
        public abstract void CreateBallCollection(int quantity);
        public abstract List<LogicApi> GetBallCollection();
        public static BallsCollectionApi CreateObjCollectionLogic(DataApi data = default(DataApi))
        {
            return new BallsCollection();
        }

    }
}
