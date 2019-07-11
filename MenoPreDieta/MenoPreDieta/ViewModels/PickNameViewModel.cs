using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using MenoPreDieta.Models;
using Xamarin.Forms;

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
            PickFirstNameCommand = new Command(async () => await PickFirstNameAsync());
            PickSecondNameCommand = new Command(async () => await PickSecondNameAsync());
            RemoveFirstNameCommand = new Command(async () => await RemoveFirstNameAsync());
            RemoveSecondNameCommand = new Command(async () => await RemoveSecondNameAsync());
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

        public Command PickFirstNameCommand { get; }

        public Command PickSecondNameCommand { get; }

        public Command RemoveFirstNameCommand { get; }

        public Command RemoveSecondNameCommand { get; }

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

            Update();
        }

        private async Task PickFirstNameAsync()
        {
            namePick.PickedNameId = namePick.FirstNameId;
            namePick.IsNamePicked = true;
            await App.Database.UpdateNamePickAsync(namePick);
            Update();
        }

        private async Task PickSecondNameAsync()
        {
            namePick.PickedNameId = namePick.SecondNameId;
            namePick.IsNamePicked = true;
            await App.Database.UpdateNamePickAsync(namePick);
            Update();
        }

        private async Task RemoveFirstNameAsync()
        {
            if (First == null) return;
            var pairsToRemove = pickPairs.Where(pair => pair.FirstNameId == First.Id || pair.SecondNameId == First.Id);
            await RemovePairs(pairsToRemove);
            var namePicks = await App.Database.GetNamePicksAsync();
            pickPairs = namePicks.Where(namePick => genderNames.Any(name => name.Id == namePick.FirstNameId)).ToList();
            Update();
        }

        private async Task RemoveSecondNameAsync()
        {
            if (Second == null) return;
            var pairsToRemove = pickPairs.Where(pair => pair.FirstNameId == Second.Id || pair.SecondNameId == Second.Id);
            await RemovePairs(pairsToRemove);
            var namePicks = await App.Database.GetNamePicksAsync();
            pickPairs = namePicks.Where(namePick => genderNames.Any(name => name.Id == namePick.FirstNameId)).ToList();
            Update();
        }

        private static async Task RemovePairs(IEnumerable<NamePickEntity> pairsToRemove)
        {
            foreach (var pairToRemove in pairsToRemove)
            {
                await App.Database.DeleteNamePickAsync(pairToRemove);
            }
        }

        private void Update()
        {
            try
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
                PairsCount = pickPairs.Count;
                RemainingPairsCount = pickPairs.Count(pickPair => !pickPair.IsNamePicked);
                Accuracy = PairsCount > 0 ? 1 - (double)RemainingPairsCount / PairsCount : 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected abstract Gender GetGender();
    }
}