using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Data
{

    public interface IBall : INotifyPropertyChanged
    {
        int ballId { get; }
        int ballSize { get; }
        double ballWeight { get; }
        double ballX { get; set; }
        double ballY { get; set; }
        double ballNewX { get; set; }
        double ballNewY { get; set; }

        void ballMove();
        void ballCreateMovementTask(int interval);
        void ballStop();
    }
    // Klasa Ball dziedziczy po klasie abstrakcyjnej DataApi i implementuje
    // metody abstrakcyjne zdefiniowane w klasie bazowej
    internal class Ball : IBall
    {
        private readonly int size;
        private readonly int id;
        private double x;
        private double y;
        private double newX;
        private double newY;
        private readonly double weight;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private Task task;
        private bool stop = false;



        public Ball(int indentyfikator, int size, double x, double y, double newX, double newY, double weight)
        {
            id = indentyfikator;
            this.size = size;
            this.x = x;
            this.y = y;
            this.newX = newX;
            this.newY = newY;
            this.weight = weight;
        }

        public int ballId { get => id; }

        public int ballSize { get => size; }

        public double ballNewX
        {
            get => newX;
            set
            {
                if (value.Equals(newX))
                {
                    return;
                }

                newX = value;
            }
        }

        public double ballNewY
        {
            get => newY;
            set
            {
                if (value.Equals(newY))
                {
                    return;
                }

                newY = value;
            }
        }

        public double ballX
        {
            get => x;
            set
            {
                if (value.Equals(x))
                {
                    return;
                }

                x = value;
                RaisePropertyChanged();
            }
        }
        public double ballY
        {
            get => y;
            set
            {
                if (value.Equals(y))
                {
                    return;
                }

                y = value;
                RaisePropertyChanged();
            }
        }
        public void ballMove()
        {
            ballX += ballNewX;
            ballY += ballNewY;
        }

        public double ballWeight { get => weight; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ballCreateMovementTask(int interval)
        {
            stop = false;
            task = Run(interval);
        }

        private async Task Run(int interval)
        {
            while (!stop)
            {
                stopwatch.Reset();
                stopwatch.Start();
                if (!stop)
                {
                    ballMove();
                }
                stopwatch.Stop();
                await Task.Delay((int)(interval - stopwatch.ElapsedMilliseconds));
            }
        }

        public void ballStop()
        {
            stop = true;
        }
    }
}