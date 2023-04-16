using System.Windows.Input;
using Model;

using System.Collections.ObjectModel;
using System.Numerics;
using Logic;

namespace ViewModel
{
    public class Controls : VM
    {
        Ball ball;

        private ObservableCollection<Ball> items;
        private static System.Timers.Timer? targetTimer;
        private static System.Timers.Timer? positionTimer;
        private string ballAmountText = "1";
        private int ballAmount = 1;
        private int fr = 50;

        public Controls()
        {

            CreateBallsButtonClick = new Commands(() => getBallVMCollection());
            AddBallButtonClick = new Commands(() => AddBallClickHandler());
            RemoveBallButtonClick = new Commands(() => RemoveBallButtonClickHandler());
            ball = new Ball();
        }

        public ICommand CreateBallsButtonClick { get; set; }
        public ICommand AddBallButtonClick { get; set; }
        public ICommand RemoveBallButtonClick { get; set; }


        private void getBallVMCollection()
        {
            if (positionTimer != null)
            {
                positionTimer.Stop();
            }
            if (targetTimer != null)
            {
                targetTimer.Stop();
            }
            Items = new ObservableCollection<Ball>();
            BallVMCollection ballVMColl = new BallVMCollection();
            Items = ballVMColl.CreateBallVMCollection(ballAmount);
            initBalls();
            initMovement();
        }

        public ObservableCollection<Ball> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                RaisePropertyChanged("Items");
            }
        }
        private void AddBallClickHandler()
        {
            if (String.IsNullOrEmpty(BallAmountText))
            {
                ballAmount = 1;
            }
            else
            {
                ballAmount++;
            }

            BallAmountText = ballAmount.ToString();
        }

        private void RemoveBallButtonClickHandler()
        {
            if (ballAmount > 1)
                ballAmount--;

            BallAmountText = ballAmount.ToString();
        }

        public string BallAmountText
        {
            get
            {
                return ballAmountText;
            }
            set
            {
                ballAmountText = value;
                ballAmount = int.Parse(ballAmountText);
                RaisePropertyChanged("BallAmountText");
            }
        }

        private void initBalls()
        {
            foreach (var x in Items)
            {
                Vector2 targetPos = ball.GetBallVMPosition();
                x.NextPosition = targetPos;
            }
            targetTimer = new System.Timers.Timer(1000);
            targetTimer.Elapsed += UpdateBallTargetPositionEvent;
            targetTimer.Start();
        }

        private void initMovement()
        {
            positionTimer = new System.Timers.Timer(800 / fr);
            positionTimer.Elapsed += BallSmoothMovementEvent;
            positionTimer.Start();
        }

        private void BallSmoothMovementEvent(object? sender, EventArgs e)
        {
            foreach (var x in Items)
            {
                if (x is Ball)
                {
                    Vector2 currentPos = new Vector2((float)x.XPos, (float)x.YPos);
                    Vector2 a = ((x.NextPosition - currentPos) / fr) + currentPos;
                    x.XPos = a.X;
                    x.YPos = a.Y;
                }
            }
        }

        private void UpdateBallTargetPositionEvent(object? sender, EventArgs e) // sets target position
        {
            foreach (var x in Items)
            {
                Vector2 targetPos = ball.GetBallVMPosition();
                x.NextPosition = targetPos;
            }
        }
    }
}
