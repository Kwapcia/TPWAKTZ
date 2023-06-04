using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Numerics;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        int ballID { get; }
        int ballSize { get; }
        double ballWeight { get; }
        Vector2 ballPosition { get; }
        Vector2 ballVelocity { get; }
        void ballChangeSpeed(Vector2 velocity);
        void moveBall(double time, ConcurrentQueue<IBall> queue); // Method to move the ball based on the elapsed time
        Task ballCreateMovementTask(int interval, ConcurrentQueue<IBall> queue); // Method to create a task for continuously moving the ball
        void saveBall(ConcurrentQueue<IBall> queue); // Method to save the current state of the ball to a queue
        void stopBall();
    }

    internal class Ball : IBall
    {
        private readonly int size;
        private readonly int id;
        private Vector2 position;
        private Vector2 velocity;
        private readonly double weight;
        private readonly Stopwatch stopwatch;
        private bool stop; // Flag to indicate if the ball movement should stop
        private readonly object locker = new object();

        public Ball(int idBall, int size, Vector2 position, Vector2 velocity, double weight)
        {
            id = idBall;
            this.size = size;
            this.position = position;
            this.velocity = velocity;
            this.weight = weight;
            stop = false;
            stopwatch = new Stopwatch();
        }

        public int ballID { get => id; }
        public int ballSize { get => size; }
        public double ballWeight { get => weight; }
        public Vector2 ballPosition { get => position; }
        public Vector2 ballVelocity { get => velocity; }

        public void ballChangeSpeed(Vector2 newVelocity)
        {
            lock (locker)
            {
                velocity = newVelocity;
            }
        }

        public void moveBall(double time, ConcurrentQueue<IBall> queue)
        {
            lock (locker)
            {
                // Update the position based on the velocity and elapsed time
                position += velocity * (float)time;
                // Notify listeners that the ballPosition property has changed
                RaisePropertyChanged(nameof(ballPosition));
                // Save the current state of the ball to the queue
                saveBall(queue);
            }
        }

        public void saveBall(ConcurrentQueue<IBall> queue)
        {
            // Enqueue a new Ball object with the current state to the queue
            queue.Enqueue(new Ball(ballID, ballSize, ballPosition, ballVelocity, ballWeight));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Invoke the PropertyChanged event to notify listeners that a property has changed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Task ballCreateMovementTask(int interval, ConcurrentQueue<IBall> queue)
        {
            stop = false;
            return Run(interval, queue);
        }

        private async Task Run(int interval, ConcurrentQueue<IBall> queue)
        {
            while (!stop)
            {
                stopwatch.Reset();
                stopwatch.Start();
                // Move the ball based on the remaining time until the next interval
                if (!stop)
                {
                    moveBall((interval - stopwatch.ElapsedMilliseconds) / 16.0, queue);
                }
                stopwatch.Stop();
                // Delay the task until the next interval
                await Task.Delay((int)(interval - stopwatch.ElapsedMilliseconds));
            }
        }

        public void stopBall()
        {
            // Set the stop flag to true, indicating that the ball movement should stop
            stop = true;
        }
    }
}
