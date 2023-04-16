namespace Logic
{
    internal class BallsCollection : BallsCollectionApi
    {
        List<LogicApi> ballCollection;
        public override void CreateBallCollection(int size)
        {
            ballCollection = new List<LogicApi>();
            for (int i = 0; i < size; i++)
            {
                Logic ball = new Logic();
                ballCollection.Add(ball.CreateBall());
            }
        }

        public override List<LogicApi> GetBallCollection()
        {
            return ballCollection;
        }
    }
}
