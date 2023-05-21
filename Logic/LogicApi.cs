using Data;
using System.Collections;
using System.ComponentModel;
using System.Numerics;

namespace Logic
{
    // abstrakcyjna klasa API logiki
    public abstract class LogicAbstractApi
    {
        // właściwość abstrakcyjna zwracająca liczbę kulek
        public abstract int getAmount { get; }

        // metoda abstrakcyjna tworząca kuleki
        public abstract IList createBalls(int count);

        // metoda abstrakcyjna rozpoczynająca ruch kulek
        public abstract void start();

        // metoda abstrakcyjna zatrzymująca ruch kulek
        public abstract void stop();

        // właściwość abstrakcyjna określająca szerokość planszy
        public abstract int width { get; set; }

        // właściwość abstrakcyjna określająca wysokość planszy
        public abstract int height { get; set; }

        // metoda abstrakcyjna zwracająca kulkę o określonym indeksie
        public abstract IBall getBall(int index);

        // metoda abstrakcyjna wywoływana po kolizji kulki z ścianą
        public abstract void collisionWithWall(IBall ball);

        // metoda abstrakcyjna wywoływana po odbiciu kulek od siebie
        public abstract void bounce(IBall ball);

        // metoda abstrakcyjna wywoływana po zmianie pozycji kuli
        public abstract void changeBallPosition(object sender, PropertyChangedEventArgs args);

        // metoda statyczna tworząca nowe API logiki
        public static LogicAbstractApi createApi(int width, int height)
        {
            return new LogicApi(width, height);
        }
    }

    // klasa implementująca API logiki
    internal class LogicApi : LogicAbstractApi
    {
        // warstwa danych
        private readonly DataAbstractApi dataLayer;

        // obiekt Mutex służący do synchronizacji dostępu do danych
        private readonly Mutex mutex = new Mutex();

        // konstruktor klasy LogicApi
        public LogicApi(int width, int height)
        {
            // tworzenie warstwy danych
            // DI
            dataLayer = DataAbstractApi.createApi(width, height);
            this.width = width;
            this.height = height;
        }

        // właściwość określająca szerokość planszy
        public override int width { get; set; }

        // właściwość określająca wysokość planszy
        public override int height { get; set; }

        // metoda rozpoczynająca ruch kulek
        public override void start()
        {
            for (int i = 0; i < dataLayer.getAmount; i++)
            {
                // tworzenie zadania ruchu dla każdej kulki
                dataLayer.getBall(i).ballCreateMovementTask(30);
            }
        }

        // Metoda stop zatrzymuje ruch wszystkich piłek w grze
        public override void stop()
        {
            for (int i = 0; i < dataLayer.getAmount; i++)
            {
                dataLayer.getBall(i).ballStop();
            }
        }

        // Metoda collisionWithWall odpowiada za detekcję kolizji piłek z krawędziami planszy
        public override void collisionWithWall(IBall ball)
        {
            double diameter = ball.BallSize;
            double right = width - diameter;
            double down = height - diameter;

            if (ball.BallPosition.X <= 0)
            {
                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
            }
            else if (ball.BallPosition.X >= right)
            {
                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
            }
            if (ball.BallPosition.Y <= 0)
            {
                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
            }
            else if (ball.BallPosition.Y >= down)
            {
                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
            }
        }

        // Metoda bounce odpowiada za detekcję kolizji między piłkami i zmianę ich kierunku i prędkości
        public override void bounce(IBall ball)
        {
            for (int i = 0; i < dataLayer.getAmount; i++)
            {
                IBall secondBall = dataLayer.getBall(i);
                if (ball.BallId == secondBall.BallId)
                {
                    continue;
                }
                if (collision(ball, secondBall))
                {
                    double m1 = ball.BallWeight;
                    double m2 = secondBall.BallWeight;
                    Vector2 v1 = ball.Velocity;
                    Vector2 v2 = secondBall.Velocity;

                    double u1x = (m1 - m2) * v1.X / (m1 + m2) + (2 * m2) * v2.X / (m1 + m2);
                    double u1y = (m1 - m2) * v1.Y / (m1 + m2) + (2 * m2) * v2.Y / (m1 + m2);
                    double u2x = 2 * m1 * v1.X / (m1 + m2) + (m2 - m1) * v2.X / (m1 + m2);
                    double u2y = 2 * m1 * v1.Y / (m1 + m2) + (m2 - m1) * v2.Y / (m1 + m2);

                    ball.Velocity = new Vector2((float)u1x, (float)u1y);
                    secondBall.Velocity = new Vector2((float)u2x, (float)u2y);
                    return;
                }
            }
        }

        // Metoda collision służy do sprawdzania, czy dwie piłki nachodzą na siebie
        internal bool collision(IBall a, IBall b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return distance(a, b) <= (a.BallSize / 2 + b.BallSize / 2);
        }

        // Metoda distance służy do obliczania odległości między środkami dwóch piłek
        internal double distance(IBall a, IBall b)
        {
            double x1 = a.BallPosition.X + a.BallSize / 2 + a.BallNewPosition.X;
            double y1 = a.BallPosition.Y + a.BallSize / 2 + a.BallNewPosition.Y;
            double x2 = b.BallPosition.X + b.BallSize / 2 + a.BallNewPosition.Y;
            double y2 = b.BallPosition.Y + b.BallSize / 2 + a.BallNewPosition.Y;
            return Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }


        // tworzy określoną liczbę kulek i dodaje je do listy kul znajdującej się w obiekcie klasy DataLayer
        public override IList createBalls(int count)
        {
            int previousCount = dataLayer.getAmount;
            // dla każdej nowo utworzonej kuli, funkcja podłącza metodę changeBallPosition do zdarzenia PropertyChanged,
            // co umożliwia automatyczne przemieszczanie kuli w czasie rzeczywistym.
            IList temp = dataLayer.createBallsList(count);
            for (int i = 0; i < dataLayer.getAmount - previousCount; i++)
            {
                dataLayer.getBall(previousCount + i).PropertyChanged += changeBallPosition;
            }
            return temp;
        }


        // zwraca ilość kul znajdujących się na liście kul w obiekcie klasy DataLayer
        public override IBall getBall(int index)
        {
            return dataLayer.getBall(index);
        }

        // zwraca ilość kul znajdujących się na liście kul w obiekcie klasy DataLayer
        public override int getAmount { get => dataLayer.getAmount; }

        //  jest metodą wywoływaną za każdym razem, gdy pozycja kuli zmienia się. Funkcja ta sprawdza,
        //  czy kula zderza się ze ścianą, a następnie wywołuje metodę bounce, aby sprawdzić, czy kula zderza się z inną kulą.
        //  Cały proces jest zabezpieczony za pomocą mutex, aby uniknąć kolizji wątków i utrzymać spójność danych.
        public override void changeBallPosition(object sender, PropertyChangedEventArgs args)
        {
            IBall ball = (IBall)sender;
            mutex.WaitOne();
            collisionWithWall(ball);
            bounce(ball);
            mutex.ReleaseMutex();
        }
    }
}
