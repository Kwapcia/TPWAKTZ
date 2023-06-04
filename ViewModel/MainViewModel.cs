using Model;
using System.Collections;
using System.Windows.Input;

namespace ViewModel
{
    public class MainWindowViewModel : VM
    {
        private readonly ModelAbstractApi modelLayer;
        private int _BallVal = 1;
        private int width;
        private int height;
        private bool _isStopEnabled = false;
        private bool isStartEnabled = false;
        private bool _isAddEnabled = true;
        private bool _isDeleteEnabled = false;
        private int size = 0;
        private IList _balls;

        public ICommand addCommand { get; set; }

        public ICommand runCommand { get; set; }

        public ICommand stopCommand { get; }

        public ICommand DeleteCommand { get; }

        public MainWindowViewModel()
        {
            width = 600;
            height = 480;
            modelLayer = ModelAbstractApi.createApi(width, height);
            stopCommand = new RelayCommand(Stop);
            addCommand = new RelayCommand(AddBalls);
            runCommand = new RelayCommand(Start);
            DeleteCommand = new RelayCommand(DeleteBalls);
        }

        public bool isStopEnabled
        {
            get { return _isStopEnabled; }
            set
            {
                _isStopEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool isRunEnabled
        {
            get { return isStartEnabled; }
            set
            {
                isStartEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool isAddEnabled
        {
            get
            {
                return _isAddEnabled;
            }
            set
            {
                _isAddEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool isDeleteEnabled
        {
            get
            {
                return _isDeleteEnabled;
            }
            set
            {
                _isDeleteEnabled = value;

                RaisePropertyChanged();
            }
        }

        public int ballValue
        {
            get
            {
                return _BallVal;
            }
            set
            {
                _BallVal = value;
                RaisePropertyChanged();
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                RaisePropertyChanged();
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                RaisePropertyChanged();
            }
        }

        private void AddBalls()
        {
            size += ballValue;
            if (size > 0)
            {
                isRunEnabled = true;
            }
            else
            {
                size = 0;
                isRunEnabled = false;
            }
            Balls = modelLayer.create(ballValue);
            ballValue = 1;
        }

        private void DeleteBalls()
        {
            size -= ballValue;
            Balls = modelLayer.delete(ballValue);
            if(size>=0)
            {
                isRunEnabled=true;
                isAddEnabled=true;
            }
            if(size<=0)
            {
                size=0;
                isAddEnabled=true;
                isRunEnabled=false;
                isDeleteEnabled=false;
            }
            ballValue=1;
        }


        private void Stop()
        {
            isStopEnabled = false;
            isAddEnabled = true;
            isRunEnabled = true;
            isDeleteEnabled = true;
            modelLayer.stop();
        }

        private void Start()
        {
            isStopEnabled = true;
            isRunEnabled = false;
            isAddEnabled = false;
             isDeleteEnabled = false;
            modelLayer.startMoving();
        }

        public IList Balls
        {
            get => _balls;
            set
            {
                if (value.Equals(_balls))
                {
                    return;
                }

                _balls = value;
                RaisePropertyChanged();
            }
        }
    }
}
