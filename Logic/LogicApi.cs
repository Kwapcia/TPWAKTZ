using Data;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;

namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public abstract IList createBalls(int count);
        public abstract IList deleteBalls(int count);
        public abstract void start();
        public abstract void stop();
        public abstract int width { get; }
        public abstract int height { get; }

        public static LogicAbstractApi createApi(int width, int height)
        {
            return new LogicApi(width, height);
        }
    }

    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi dataLayer;
        private ObservableCollection<IBall> balls { get; }
        private readonly ConcurrentQueue<IBall> queue;
        public LogicApi(int width, int height)
        {
            dataLayer = DataAbstractApi.createApi(width, height);
            this.width = width;
            this.height = height;
            // Inicjalizuje listę piłek i kolejkę
            balls = new ObservableCollection<IBall>();
            queue = new ConcurrentQueue<IBall>();
        }

        public override int width { get; }

        public override int height { get; }

        public override void start()
        {
            // Tworzy zadanie logowania przy użyciu warstwy danych
            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].PropertyChanged += ballPositionChanged;
                balls[i].ballCreateMovementTask(30, queue);
            }
            // Tworzy zadanie logowania przy użyciu warstwy danych
            dataLayer.createLoggingTask(queue);
        }

        public override void stop()
        {
           // Zatrzymuje piłki i usuwa obsługę zdarzeń
            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].stopBall();
                balls[i].PropertyChanged -= ballPositionChanged;
            }
        }

        // Tworzy określoną liczbę piłek, zapewniając brak kolizji z istniejącymi piłkami
        public override IList createBalls(int count)
        {
            int liczba = balls.Count;
             // Tworzy nowe piłki i sprawdza kolizje z istniejącymi piłkami
            for (int i = liczba; i < liczba + count; i++)
            {
                bool contain = true;
                bool licz;
                while (contain)
                {
                    balls.Add(dataLayer.createBall(i + 1));
                    licz = false;
                    for (int j = 0; j < i; j++)
                    {
                        // Sprawdza kolizję między piłkami
                        if (balls[i].ballPosition.X <= balls[j].ballPosition.X + balls[j].ballSize && balls[i].ballPosition.X + balls[i].ballSize >= balls[j].ballPosition.X)
                        {
                            if (balls[i].ballPosition.Y <= balls[j].ballPosition.Y + balls[j].ballSize && balls[i].ballPosition.Y + balls[i].ballSize >= balls[j].ballPosition.Y)
                            {
                                // Jeśli występuje kolizja, usuwa nowo utworzoną piłkę
                                licz = true;
                                balls.Remove(balls[i]);
                                break;
                            }
                        }
                    }
                    if (!licz)
                    {
                        contain = false;
                    }
                }
            }
            return balls;
        }


        public override IList deleteBalls(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (balls.Count > 0)
                {
                    balls.Remove(balls[balls.Count - 1]);
                };
            }
            return balls;
        }

       // Obsługuje kolizję piłki z ścianami
        internal void wallCollision(IBall ball)
        {
            double diameter = ball.ballSize;
            double right = width - diameter;
            double down = height - diameter;
            // Sprawdza i obsługuje kolizję z ścianami
            if (ball.ballPosition.X <= 5)
            {
                if (ball.ballVelocity.X <= 0)
                {
                    ball.ballChangeSpeed(new Vector2(-ball.ballVelocity.X, ball.ballVelocity.Y));
                }
            }
            else if (ball.ballPosition.X >= right - 5)
            {
                if (ball.ballVelocity.X > 0)
                {
                    ball.ballChangeSpeed(new Vector2(-ball.ballVelocity.X, ball.ballVelocity.Y));
                }
            }
            if (ball.ballPosition.Y <= 5)
            {
                if (ball.ballVelocity.Y <= 0)
                {
                    ball.ballChangeSpeed(new Vector2(ball.ballVelocity.X, -ball.ballVelocity.Y));
                }
            }
            else if (ball.ballPosition.Y >= down - 5)
            {
                if (ball.ballVelocity.Y > 0)
                {
                    ball.ballChangeSpeed(new Vector2(ball.ballVelocity.X, -ball.ballVelocity.Y));
                }
            }
        }

        // Obsługuje odbicie piłki od innych piłek
        internal void ballBounce(IBall ball)
        {
            lock (ball)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    IBall secondBall = balls[i];
                    if (ball.ballID == secondBall.ballID)
                    {
                        continue;
                    }
                    lock (secondBall)
                    {
                        if (collision(ball, secondBall))
                        {
                            Vector2 relativePosition = ball.ballPosition - secondBall.ballPosition;
                            Vector2 relativeVelocity = ball.ballVelocity - secondBall.ballVelocity;

                            if (Vector2.Dot(relativePosition, relativeVelocity) > 0)
                            {
                                return;
                            }

                            double m1 = ball.ballWeight;
                            Vector2 v1 = ball.ballVelocity;

                            double m2 = secondBall.ballWeight;
                            Vector2 v2 = secondBall.ballVelocity;

                            Vector2 u1 = Vector2.Multiply((Vector2.Multiply((float)(m1 - m2), v1) + Vector2.Multiply((float)(2 * m2), v2)), (float)(1 / (m1 + m2)));
                            Vector2 u2 = Vector2.Multiply((Vector2.Multiply((float)(2 * m1), v1) + Vector2.Multiply((float)(m2 - m1), v2)), (float)(1 / (m1 + m2)));

                            secondBall.ballChangeSpeed(u2);
                            ball.ballChangeSpeed(u1);
                        }
                    }
                }
            }
        }

        // Sprawdza kolizję między dwoma piłkami
        internal bool collision(IBall a, IBall b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return distance(a, b) <= (a.ballSize / 2 + b.ballSize / 2);
        }

        // Oblicza odległość między dwoma piłkami
        internal double distance(IBall a, IBall b)
        {
            Vector2 centerA = a.ballPosition + new Vector2(a.ballSize / 2);
            Vector2 centerB = b.ballPosition + new Vector2(b.ballSize / 2);
            return Vector2.Distance(centerA, centerB);
        }

        // Obsługuje zmianę pozycji piłki
        internal void ballPositionChanged(object sender, PropertyChangedEventArgs args)
        {
            IBall ball = (IBall)sender;
            lock (ball)
            {
                wallCollision(ball);
                ballBounce(ball);
            }
        }

    }
}
