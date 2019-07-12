using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Annotations;
using MenoPreDieta.Dialogs;
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
        private bool isBusy;
        private bool isEnabled;
        private Color genderColor;
        private readonly IConfirmationDialog confirmationDialog;

        protected PickNameViewModel([NotNull] IConfirmationDialog confirmationDialog)
        {
            this.confirmationDialog = confirmationDialog ?? throw new ArgumentNullException(nameof(confirmationDialog));
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

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (value == isBusy) return;
                isBusy = value;
                IsEnabled = !value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (value == isEnabled) return;
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Color GenderColor
        {
            get => genderColor;
            set
            {
                if (value.Equals(genderColor)) return;
                genderColor = value;
                OnPropertyChanged();
            }
        }

        public Command PickFirstNameCommand { get; }

        public Command PickSecondNameCommand { get; }

        public Command RemoveFirstNameCommand { get; }

        public Command RemoveSecondNameCommand { get; }

        public abstract Command ShowRankedNamesCommand { get; }

        public abstract Command ResetCommand { get; }

        public virtual async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                var names = await App.Database.GetNamesAsync();
                var gender = GetGender();
                genderNames = names.Where(name => name.Gender == gender).ToList();
                NamesCount = genderNames.Count;
                var namePicks = await App.Database.GetNamePicksAsync();
                pickPairs = namePicks.Where(namePick => namePick.Gender == gender)
                    .ToList();
                if (!pickPairs.Any())
                {
                    for (int i = 0; i < genderNames.Count; i++)
                    {
                        for (int j = i + 1; j < genderNames.Count; j++)
                        {
                            var firstName = genderNames[i];
                            var secondName = genderNames[j];
                            var namePick = new NamePickEntity
                            {
                                FirstNameId = firstName.Id,
                                SecondNameId = secondName.Id,
                                Gender = gender
                            };
                            pickPairs.Add(namePick);
                        }
                    }

                    await App.Database.InsertNamePicksAsync(pickPairs);
                }

                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected abstract Gender GetGender();

        private async Task PickFirstNameAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                namePick.PickedNameId = namePick.FirstNameId;
                namePick.IsNamePicked = true;
                await App.Database.UpdateNamePickAsync(namePick);
                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task PickSecondNameAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                namePick.PickedNameId = namePick.SecondNameId;
                namePick.IsNamePicked = true;
                await App.Database.UpdateNamePickAsync(namePick);
                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RemoveFirstNameAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (First == null) return;
                var pairsToRemove = pickPairs.Where(pair => pair.FirstNameId == First.Id || pair.SecondNameId == First.Id);
                await RemovePairs(pairsToRemove);
                var namePicks = await App.Database.GetNamePicksAsync();
                pickPairs = namePicks.Where(namePick => namePick.Gender == GetGender()).ToList();
                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RemoveSecondNameAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (Second == null) return;
                var pairsToRemove = pickPairs.Where(pair => pair.FirstNameId == Second.Id || pair.SecondNameId == Second.Id);
                await RemovePairs(pairsToRemove);
                var namePicks = await App.Database.GetNamePicksAsync();
                pickPairs = namePicks.Where(namePick => namePick.Gender == GetGender()).ToList();
                Update();
            }
            finally
            {
                IsBusy = false;
            }
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
            Accuracy = PairsCount > 0 ? 1 - (double) RemainingPairsCount / PairsCount : 0;
        }

        protected async Task ResetAsync()
        {
            if (await confirmationDialog.ShowDialog())
            {
                await App.Database.DeleteNamePicksAsync(pickPairs);
                await LoadAsync();
            }
        }
    }
}