using Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    // klasa abstrakcyjna dla interfejsu modelu
    public abstract class ModelAbstractApi
    {
        // Właściwości abstrakcyjne do zaimplementowania przez klasy dziedziczące
        public abstract int width { get; }
        public abstract int height { get; }
        public abstract void startMoving();
        public abstract IList start(int ballVal);
        public abstract void stop();

        // statyczna metoda fabryczna do tworzenia obiektów ModelAbstractApi
        public static ModelAbstractApi createApi(int Weight, int Height)
        {
            return new ModelApi(Weight, Height);
        }
    }

    // klasa implementująca interfejs ModelAbstractApi
    internal class ModelApi : ModelAbstractApi
    {
        // Właściwości tylko do odczytu dla szerokości i wysokości planszy
        public override int width { get; }
        public override int height { get; }
        // Pole logicLayer przechowujące obiekt interfejsu LogicAbstractApi
        private readonly LogicAbstractApi logicLayer;

        // Konstruktor inicjalizujący pola i tworzący nowy obiekt LogicAbstractApi
        public ModelApi(int Width, int Height)
        {
            width = Width;
            height = Height;
            logicLayer = LogicAbstractApi.createApi(width, height);
        }

        // Implementacja metody startMoving z interfejsu ModelAbstractApi
        public override void startMoving()
        {
            logicLayer.start();
        }

        // Implementacja metody stop z interfejsu ModelAbstractApi
        public override void stop()
        {
            logicLayer.stop();
        }

        // Implementacja metody start z interfejsu ModelAbstractApi
        public override IList start(int ballVal) => logicLayer.createBalls(ballVal);

    }
}
