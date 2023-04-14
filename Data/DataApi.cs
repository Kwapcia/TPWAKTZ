using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataApi

    {
        public abstract double getXPosition();
        public abstract double getYPosition();
        public abstract void setXPosition(double x);
        public abstract void setYPosition(double y);
        public static DataApi CreateBall(float X, float Y)
        {
            return new Ball(X, Y);
        }
    }
}
