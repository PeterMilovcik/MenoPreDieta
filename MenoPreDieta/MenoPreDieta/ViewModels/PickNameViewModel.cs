﻿using System;
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
    public abstract class PickNameViewModel<TNameEntity, TNamePickEntity> : ViewModel
        where TNameEntity : INameEntity
        where TNamePickEntity : INamePickEntity
    {
        private NameModel first;
        private NameModel second;
        private int namesCount;
        private int pairsCount;
        private int remainingPairsCount;
        private double accuracy;
        private List<TNameEntity> names;
        private List<TNamePickEntity> pickPairs;
        private readonly Random random;
        private TNamePickEntity namePick;
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
                names = await GetNamesAsync();
                NamesCount = names.Count;
                pickPairs = await GetNamePicksAsync();
                if (!pickPairs.Any())
                {
                    for (int i = 0; i < names.Count; i++)
                    {
                        for (int j = i + 1; j < names.Count; j++)
                        {
                            var firstName = names[i];
                            var secondName = names[j];
                            pickPairs.Add(CreateNamePickEntity(firstName.Id, secondName.Id));
                        }
                    }

                    await InsertToDatabase(pickPairs);
                }

                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected abstract Task InsertToDatabase(List<TNamePickEntity> namePicks);

        protected abstract TNamePickEntity CreateNamePickEntity(int firstId, int secondId);

        protected abstract Task<List<TNameEntity>> GetNamesAsync();

        protected abstract Task<List<TNamePickEntity>> GetNamePicksAsync();

        private async Task PickFirstNameAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (namePick != null)
                {
                    namePick.PickedNameId = namePick.FirstNameId;
                    namePick.IsNamePicked = true;
                    await UpdateNamePickAsync(namePick);
                }

                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected abstract Task UpdateNamePickAsync(TNamePickEntity namePickEntity);

        private async Task PickSecondNameAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (namePick != null)
                {
                    namePick.PickedNameId = namePick.SecondNameId;
                    namePick.IsNamePicked = true;
                    await UpdateNamePickAsync(namePick);
                }

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
                if (First == null || Second == null) return;
                var pairsToRemove =
                    pickPairs.Where(pair => pair.FirstNameId == First.Id || pair.SecondNameId == First.Id);
                await RemovePairs(pairsToRemove);
                pickPairs = await GetNamePicksAsync();

                var notPickedNamePairs = pickPairs
                    .Where(pair => !pair.IsNamePicked && 
                                   pair.SecondNameId == Second.Id).ToList();
                if (notPickedNamePairs.Any())
                {
                    namePick = notPickedNamePairs[random.Next(notPickedNamePairs.Count - 1)];
                    var firstName = names.Single(name => name.Id == namePick.FirstNameId);
                    First = new NameModel(firstName.Id, firstName.Value);
                    var secondName = names.Single(name => name.Id == namePick.SecondNameId);
                    Second = new NameModel(secondName.Id, secondName.Value);

                    PairsCount = pickPairs.Count;
                    RemainingPairsCount = pickPairs.Count(pickPair => !pickPair.IsNamePicked);
                    Accuracy = PairsCount > 0 ? 1 - (double) RemainingPairsCount / PairsCount : 0;
                }
                else
                {
                    Update();
                }
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
                if (Second == null || First == null) return;
                var pairsToRemove = pickPairs.Where(pair => pair.FirstNameId == Second.Id || pair.SecondNameId == Second.Id);
                await RemovePairs(pairsToRemove);
                pickPairs = await GetNamePicksAsync();

                var notPickedNamePairs = pickPairs
                    .Where(pair => !pair.IsNamePicked &&
                                   pair.FirstNameId == First.Id).ToList();
                if (notPickedNamePairs.Any())
                {
                    namePick = notPickedNamePairs[random.Next(notPickedNamePairs.Count - 1)];
                    var firstName = names.Single(name => name.Id == namePick.FirstNameId);
                    First = new NameModel(firstName.Id, firstName.Value);
                    var secondName = names.Single(name => name.Id == namePick.SecondNameId);
                    Second = new NameModel(secondName.Id, secondName.Value);

                    PairsCount = pickPairs.Count;
                    RemainingPairsCount = pickPairs.Count(pickPair => !pickPair.IsNamePicked);
                    Accuracy = PairsCount > 0 ? 1 - (double)RemainingPairsCount / PairsCount : 0;
                }
                else
                {
                    Update();
                }

            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RemovePairs(IEnumerable<TNamePickEntity> pairsToRemove)
        {
            foreach (var pairToRemove in pairsToRemove)
            {
                await DeleteNamePicksAsync(pairToRemove);
            }
        }

        protected abstract Task DeleteNamePicksAsync(TNamePickEntity namePickEntity);

        private void Update()
        {
            var notPickedNamePairs = pickPairs.Where(pair => !pair.IsNamePicked).ToList();
            if (notPickedNamePairs.Any())
            {
                namePick = notPickedNamePairs[random.Next(notPickedNamePairs.Count - 1)];
                var firstName = names.Single(name => name.Id == namePick.FirstNameId);
                First = new NameModel(firstName.Id, firstName.Value);
                var secondName = names.Single(name => name.Id == namePick.SecondNameId);
                Second = new NameModel(secondName.Id, secondName.Value);
            }
            else
            {
                ShowRankedNamesCommand.Execute(null);
            }

            PairsCount = pickPairs.Count;
            RemainingPairsCount = pickPairs.Count(pickPair => !pickPair.IsNamePicked);
            Accuracy = PairsCount > 0 ? 1 - (double) RemainingPairsCount / PairsCount : 0;
        }

        protected async Task ResetAsync()
        {
            if (await confirmationDialog.ShowDialog())
            {
                await RecreateTableAsync();
                await LoadAsync();
            }
        }

        protected abstract Task RecreateTableAsync();
    }
}