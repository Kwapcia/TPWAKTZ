using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    internal class BallVMCollection
    {
        public ObservableCollection<Ball> CreateBallVMCollection(int quantity)
        {
            // Utworzenie obiektu BallModelCollection i wywołanie metody tworzącej kolekcję Modeli piłek
            BallModelCollection bMC = new BallModelCollection();
            bMC.CreateBallModelCollection(quantity);
            // Pobranie utworzonej kolekcji Modeli piłek
            List<BallModel> ballCollection = bMC.GetBallModelCollection();

            // Utworzenie pustej kolekcji ViewModeli piłek
            ObservableCollection<Ball> ballVMCollection = new ObservableCollection<Ball>();

            // Iteracja przez kolekcję Modeli piłek i tworzenie ViewModeli na ich podstawie
            foreach (BallModel ballM in ballCollection)
            {
                // Utworzenie ViewModelu piłki na podstawie Modelu
                Ball ballVM = new Ball(ballM);

                // Ustawienie właściwości pozycji ViewModelu na wartości pozycji Modelu
                ballVM.XPos = ballM.ModelXPosition;
                ballVM.YPos = ballM.ModelYPosition;

                // Dodanie utworzonego ViewModelu do kolekcji ViewModeli piłek
                ballVMCollection.Add(ballVM);
            }

            // Zwrócenie utworzonej kolekcji ViewModeli piłek
            return ballVMCollection;

        }
    }
}
