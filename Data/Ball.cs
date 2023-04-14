using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    // Klasa Ball dziedziczy po klasie abstrakcyjnej DataApi i implementuje
    // metody abstrakcyjne zdefiniowane w klasie bazowej
    internal class Ball : DataApi
    {
        private double positionX;
        private double positionY;

        public Ball(double x, double y)
        {
            positionX = x;
            positionY = y;
        }

        public override double getXPosition()
        {
            return positionX;
        }

        public override double getYPosition()
        {
            return positionY;
        }

        public override void setXPosition(double newX)
        {
            positionX = newX;
        }

        public override void setYPosition(double newY)
        {
            positionY = newY;
        }

        public double X
        {
            get
            {
                return positionX;
            }
            set
            {
                positionX = value;
            }
        }
        public double Y
        {
            get
            {
                return positionY;
            }
            set
            {
                positionY = value;
            }
        }
    }
}