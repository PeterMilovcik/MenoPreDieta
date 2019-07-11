using MenoPreDieta.Models;

namespace MenoPreDieta.ViewModels
{
    public class PickNameViewModel : ViewModel
    {
        private NameModel first;
        private NameModel second;

        public NameModel First
        {
            get => first;
            set
            {
                if (Equals(value, first)) return;
                first = value;
                OnPropertyChanged();
            }
        }

        public NameModel Second
        {
            get => second;
            set
            {
                if (Equals(value, second)) return;
                second = value;
                OnPropertyChanged();
            }
        }
    }
}