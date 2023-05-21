using System.Collections;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Data
{

    public abstract class DataAbstractApi
    {
        public abstract int getAmount { get; }
        public abstract IList createBallsList(int count);
        public abstract int width { get; }
        public abstract int height { get; }
        public abstract IBall getBall(int index);

        public static DataAbstractApi createApi(int width, int height)
        {
            return new DataApi(width, height);
        }
    }

    internal class DataApi : DataAbstractApi
    {
        private ObservableCollection<IBall> balls { get; }
        private readonly Mutex mutex = new Mutex();

        private readonly Random random = new Random();

        public override int width { get; }
        public override int height { get; }

        public DataApi(int width, int height)
        {
            balls = new ObservableCollection<IBall>();
            this.width = width;
            this.height = height;
        }

        public ObservableCollection<IBall> Balls => balls;

        public override IList createBallsList(int count)
        {
            if (count > 0)
            {
                int ballsCount = balls.Count;
                for (int i = 0; i < count; i++)
                {
                    mutex.WaitOne();
                    int r = 20;
                    int pom = random.Next(20, 40);
                    double weight = pom;
                    double x = random.Next(r, width - r);
                    double y = random.Next(r, height - r);
                    double newX = random.Next(-10, 10) + random.NextDouble();
                    double newY = random.Next(-10, 10) + random.NextDouble();
                    Vector2 position = new Vector2((float)x, (float)y);
                    Vector2 newPosition = new Vector2((float)newX, (float)newY);
                    Vector2 velocity = new Vector2((float)newX, (float)newY); // Przykładowa wartość dla velocity
                    Ball ball = new Ball(i + 1 + ballsCount, r, position, newPosition, velocity, weight);

                    balls.Add(ball);
                    mutex.ReleaseMutex();
                }
            }
            if (count < 0)
            {
                for (int i = count; i < 0; i++)
                {
                    if (balls.Count > 0)
                    {
                        mutex.WaitOne();
                        balls.Remove(balls[balls.Count - 1]);
                        mutex.ReleaseMutex();
                    }
                }
            }
            return balls;
        }


        public override int getAmount { get => balls.Count; }

        public override IBall getBall(int index)
        {
            return balls[index];
        }
    }
}
