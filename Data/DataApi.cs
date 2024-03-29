﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataAbstractApi
    {
        public abstract IBall createBall(int count);

        public abstract int width { get; }

        public abstract int height { get; }

        public abstract void stopLoggingTask();

        public abstract Task createLoggingTask(ConcurrentQueue<IBall> logQueue);

        public abstract void appendObjectToJsonFile(string filename, string newJsonObject);

        public static DataAbstractApi createApi(int width, int height)
        {
            return new DataApi(width, height);
        }
    }

    internal class DataApi : DataAbstractApi
    {
        private readonly Random random = new Random();
        private readonly Stopwatch stopwatch;
        private readonly string logPath = "Log.json";
        private bool newSession;
        private bool stop;

        public override int width { get; }

        public override int height { get; }

        public DataApi(int width, int height)
        {
            this.width = width;
            this.height = height;
            newSession = true;
            stopwatch = new Stopwatch();
        }

        public override IBall createBall(int count)
        {
            int radius = 30;
            double weight = radius;
            Vector2 position = new Vector2(
                random.Next(radius + 20, width - radius - 20),
                random.Next(radius + 20, height - radius - 20)
            );
            Vector2 velocity = new Vector2(0, 0);
            while (velocity.X == 0)
            {
                velocity.X = (float)(random.Next(-5, 5) + random.NextDouble());
            }
            while (velocity.Y == 0)
            {
                velocity.Y = (float)(random.Next(-5, 5) + random.NextDouble());
            }
            Ball ball = new Ball(count, radius, position, velocity, weight);
            return ball;
        }

        // Zatrzymuje zadanie logowania
        public override void stopLoggingTask()
        {
            stop = true;
        }

        // Tworzy zadanie logowania z określoną kolejką logów
        public override Task createLoggingTask(ConcurrentQueue<IBall> logQueue)
        {
            stop = false;
            return callLogger(logQueue);
        }

        // Usuwa istniejący plik dziennika, jeśli istnieje i rozpoczyna nową sesję
        internal void FileMaker(string filename)
        {
            if (File.Exists(filename) && newSession)
            {
                newSession = false;
                File.Delete(filename);
            }
        }

       // Dodaje nowy obiekt JSON do określonego pliku JSON (usuwa jeśli już istniał)
        public override void appendObjectToJsonFile(string filename, string newJsonObject)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine("[]");
            }
            string content;
            using (StreamReader sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            content = content.TrimEnd();
            content = content.Remove(content.Length - 1, 1);
            if (content.Length == 1)
            {
                content = String.Format("{0}\n{1}\n]\n", content.Trim(), newJsonObject);
            }
            else
            {
                content = String.Format("{0},\n{1}\n]\n", content.Trim(), newJsonObject);
            }
            using (StreamWriter sw = File.CreateText(filename))
            {
                sw.Write(content);
            }
        }

        // Wywołuje zadanie logowania asynchronicznie
        internal async Task callLogger(ConcurrentQueue<IBall> logQueue)
{
    FileMaker(logPath);
    string diagnostics;
    string date;
    string log;
    ManualResetEvent queueNotEmpty = new ManualResetEvent(false);

    while (!stop)
    {
        stopwatch.Reset();
        stopwatch.Start();
        IBall logObject;

        if (!logQueue.IsEmpty)
        {
            while (!logQueue.TryDequeue(out logObject))
            {
                // Delay until the logQueue is not empty
                queueNotEmpty.Reset();
                queueNotEmpty.WaitOne();
            }

            diagnostics = $"{{\"ballID\":{logObject.ballID},\"ballSize\":{logObject.ballSize},\"ballWeight\":{logObject.ballWeight},\"ballPosition\":{{\"X\":{logObject.ballPosition.X},\"Y\":{logObject.ballPosition.Y}}},\"ballVelocity\":{{\"X\":{logObject.ballVelocity.X},\"Y\":{logObject.ballVelocity.Y}}}}}";
            date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");
            log = "{" + String.Format("\n\t\"Date\": \"{0}\",\n\t\"Info\":{1}\n", date, diagnostics) + "}";

            lock (this)
            {
                File.AppendAllText(logPath, log);
            }

            stopwatch.Stop();
            await Task.Delay((int)(stopwatch.ElapsedMilliseconds));
        }
        else
        {
            // Set the queueNotEmpty event to allow waiting threads to continue
            queueNotEmpty.Set();
            await Task.Delay(100); // Delay before checking the logQueue again
        }
    }
}
    }
}
