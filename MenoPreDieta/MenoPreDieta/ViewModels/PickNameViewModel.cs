using System;
using System.Collections.Generic;
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
        private int remainingPairsCount;
        private double accuracy;
        private List<NameEntity> genderNames;
        private List<NamePickEntity> pickPairs;
        private readonly Random random;
        private NamePickEntity namePick;

        protected PickNameViewModel()
        {
            random = new Random();
        }

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

        public int RemainingPairsCount
        {
            get => remainingPairsCount;
            set
            {
                if (value == remainingPairsCount) return;
                remainingPairsCount = value;
                OnPropertyChanged();
            }
        }

        public double Accuracy
        {
            get => accuracy;
            set
            {
                if (value.Equals(accuracy)) return;
                accuracy = value;
                OnPropertyChanged();
            }
        }

        public virtual async Task LoadAsync()
        {
            var names = await App.Database.GetNamesAsync();
            genderNames = names.Where(name => name.Gender == GetGender()).ToList();
            NamesCount = genderNames.Count;
            var namePicks = await App.Database.GetNamePicksAsync();
            pickPairs = namePicks.Where(namePick => genderNames.Any(name => name.Id == namePick.FirstNameId)).ToList();
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
            RemainingPairsCount = pickPairs.Count(pickPair => !pickPair.IsNamePicked);
            Accuracy = PairsCount > 0 ? 1 - (double) RemainingPairsCount / PairsCount : 0;

            ChooseNamesToPick();
        }

        private void ChooseNamesToPick()
        {
            var notPickedNamePairs = pickPairs.Where(pair => !pair.IsNamePicked).ToList();
            if (notPickedNamePairs.Any())
            {
                namePick = notPickedNamePairs[random.Next(notPickedNamePairs.Count - 1)];
                var firstName = genderNames.Single(name => name.Id == namePick.FirstNameId);
                First = new NameModel(firstName.Id, firstName.Value);
                var secondName = genderNames.Single(name => name.Id == namePick.SecondNameId);
                Second = new NameModel(secondName.Id, secondName.Value);
            }
            else
            {
                namePick = null;
            }
        }

        protected abstract Gender GetGender();
    }
}