using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using MenoPreDieta.Models;

namespace MenoPreDieta.ViewModels
{
    public abstract class PickNameViewModel : ViewModel
    {
        private NameModel first;
        private NameModel second;
        private int namesCount;
        private int pairsCount;

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

        public int NamesCount
        {
            get => namesCount;
            set
            {
                if (value == namesCount) return;
                namesCount = value;
                OnPropertyChanged();
            }
        }

        public int PairsCount
        {
            get => pairsCount;
            set
            {
                if (value == pairsCount) return;
                pairsCount = value;
                OnPropertyChanged();
            }
        }

        public virtual async Task LoadAsync()
        {
            var names = await App.Database.GetNamesAsync();
            var genderNames = names.Where(name => name.Gender == GetGender()).ToList();
            NamesCount = genderNames.Count;
            var namePicks = await App.Database.GetNamePicksAsync();
            var pickPairs = namePicks.Where(namePick => genderNames.Any(name => name.Id == namePick.FirstNameId)).ToList();
            if (!pickPairs.Any())
            {
                for (int i = 0; i < genderNames.Count; i++)
                {
                    for (int j = i + 1; j < genderNames.Count; j++)
                    {
                        var firstName = genderNames[i];
                        var secondName = genderNames[j];
                        var namePick = new NamePickEntity {FirstNameId = firstName.Id, SecondNameId = secondName.Id};
                        pickPairs.Add(namePick);
                    }
                }

                await App.Database.InsertNamePicksAsync(pickPairs);
            }

            PairsCount = pickPairs.Count;
        }

        protected abstract Gender GetGender();
    }
}