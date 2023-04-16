﻿using Model;
using System.Numerics;


namespace ViewModel
{
    public class Ball : VM
    {
        private BallModel ball; // model piłki
        private double r = 15; // promień piłki
        private double X; // położenie piłki na osi X
        private double Y; // położenie piłki na osi Y

        // konstruktor domyślny
        public Ball()
        {
            ball = new BallModel();
        }

        // konstruktor z modelem piłki jako argument
        public Ball(BallModel ballModel)
        {
            X = ballModel.ModelXPosition;
            Y = ballModel.ModelYPosition;
            ball = new BallModel();
        }

        // właściwość pozwalająca pobrać i ustawić następne położenie piłki
        public Vector2 NextPosition { get; set; }

        // właściwość pozwalająca pobrać i ustawić wektor przesunięcia piłki
        public Vector2 NextStepVector { get; set; }

        // właściwość pozwalająca pobrać średnicę piłki
        public double BallDiameter
        {
            get
            {
                return r * 2;
            }
        }

        // właściwość pozwalająca pobrać i ustawić położenie piłki na osi X
        public double XPos
        {
            get
            {
                return ball.ModelXPosition;
            }
            set
            {
                ball.ModelXPosition = value;
                RaisePropertyChanged("XPos"); // aktualizacja powiązanych z tą właściwością elementów interfejsu użytkownika
            }
        }

        // właściwość pozwalająca pobrać i ustawić położenie piłki na osi Y
        public double YPos
        {
            get
            {
                return ball.ModelYPosition;
            }
            set
            {
                ball.ModelYPosition = value;
                RaisePropertyChanged("YPos"); // aktualizacja powiązanych z tą właściwością elementów interfejsu użytkownika
            }
        }

        // metoda zwracająca wektor 2D reprezentujący położenie piłki w interfejsie użytkownika
        public Vector2 getPosBallVM()
        {
            return new Vector2((float)XPos, (float)YPos);
        }

        // metoda wykorzystująca metodę GetBallPosition() z modelu piłki, aby zwrócić wektor 2D reprezentujący położenie piłki na planszy
        public Vector2 GetBallVMPosition()
        {
            return ball.GetBallPosition();
        }

        // właściwość pozwalająca pobrać i ustawić promień piłki
        public double Radius
        {
            get
            {
                return r;
            }
            set
            {
                r = value;
            }
        }
    }
}