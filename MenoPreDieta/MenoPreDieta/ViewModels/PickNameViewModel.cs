using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Annotations;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Extensions;
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
        private readonly Random random;
        private bool isBusy;
        private bool isEnabled;
        private readonly IConfirmationDialog confirmationDialog;

        protected PickNameViewModel([NotNull] IConfirmationDialog confirmationDialog)
        {
            this.confirmationDialog = confirmationDialog ?? throw new ArgumentNullException(nameof(confirmationDialog));
            random = new Random();
            PickFirstNameCommand = new Command(async () => await PickFirstNameAsync());
            PickSecondNameCommand = new Command(async () => await PickSecondNameAsync());
            RemoveFirstNameCommand = new Command(async () => await RemoveFirstNameAsync());
            RemoveSecondNameCommand = new Command(async () => await RemoveSecondNameAsync());
            ResetCommand = new Command(async () => await ResetAsync());
            MessagingCenter.Subscribe<RankedBoyNamesViewModel>(
                this, "ResetBoyNamePicks", sender => Initialize());
            MessagingCenter.Subscribe<RankedGirlNamesViewModel>(
                this, "ResetGirlNamePicks", sender => Initialize());
            MessagingCenter.Subscribe<RestoreBoyNameViewModel>(
                this, "RestoreBoyName", sender => Initialize());
            MessagingCenter.Subscribe<RestoreGirlNameViewModel>(
                this, "RestoreGirlName", sender => Initialize());
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

        public Command PickFirstNameCommand { get; }

        public Command PickSecondNameCommand { get; }

        public Command RemoveFirstNameCommand { get; }

        public Command RemoveSecondNameCommand { get; }

        public Command ResetCommand { get; }

        public abstract Command ShowRankedNamesCommand { get; }

        public abstract Command RestoreCommand { get; }

        public void Initialize()
        {
            try
            {
                IsBusy = true;
                NamesCount = App.Names.Catalog.Count;
                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task PickFirstNameAsync() => 
            await PickNameAsync(App.Names.Pairs.Selected.FirstNameId);

        private async Task PickSecondNameAsync() => 
            await PickNameAsync(App.Names.Pairs.Selected.SecondNameId);

        private async Task PickNameAsync(int nameId)
        {
            if (App.Names.Pairs.Selected != null)
            {
                App.Names.Pairs.Selected.PickedNameId = nameId;
                App.Names.Pairs.Selected.IsNamePicked = true;
                App.Names.Update(App.Names.Pairs.Selected);
                Update();
                await App.Names.ProcessUpdateQueueAsync();
            }
        }

        private async Task RemoveFirstNameAsync()
        {
            if (First == null) return;
            var nameId = First.Id;
            First = null;
            await RemoveNameAsync(nameId);
        }

        private async Task RemoveSecondNameAsync()
        {
            if (Second == null) return;
            var nameId = Second.Id;
            Second = null;
            await RemoveNameAsync(nameId);
        }

        private async Task RemoveNameAsync(int nameId)
        {
            var pairsToRemove = App.Names.Pairs.With(nameId);
            pairsToRemove.ForEach(item =>
            {
                App.Names.Delete(item);
            });
            Update();
            await App.Names.ProcessDeleteQueueAsync();
            MessagingCenter.Send(this, "NameDeleted");
        }

        private void Update()
        {
            var notPickedNamePairs = App.Names.Pairs.NotPicked();
            App.Names.DeleteQueue.ForEach(item => notPickedNamePairs.Remove(item));
            App.Names.UpdateQueue.ForEach(item => notPickedNamePairs.Remove(item));
            if (notPickedNamePairs.Any())
            {
                var pairsToPick = new List<INamePickEntity>();
                if (First == null && Second != null)
                {
                    pairsToPick = notPickedNamePairs.Where(pair => pair.SecondNameId == Second.Id).ToList();
                }
                if (First != null && Second == null)
                {
                    pairsToPick = notPickedNamePairs.Where(pair => pair.FirstNameId == First.Id).ToList();
                }

                var pairs = pairsToPick.Any() ? pairsToPick : notPickedNamePairs;
                App.Names.Pairs.Selected = pairs.RandomItem();
                UpdateFirstAndSecondName();
            }
            else
            {
                ShowRankedNamesCommand.Execute(null);
            }

            UpdateStats();
        }

        private void UpdateFirstAndSecondName()
        {
            First = App.Names.Catalog.NameWithId(App.Names.Pairs.Selected.FirstNameId).ToNameModel();
            Second = App.Names.Catalog.NameWithId(App.Names.Pairs.Selected.SecondNameId).ToNameModel();
        }

        private void UpdateStats()
        {
            PairsCount = App.Names.Pairs.Count;
            RemainingPairsCount = App.Names.Pairs.NotPicked().Count;
            Accuracy = PairsCount > 0 ? 1 - (double) RemainingPairsCount / PairsCount : 0;
        }

        private INamePickEntity GetRandomNamePick(IReadOnlyList<INamePickEntity> list) => 
            list[random.Next(list.Count - 1)];

        protected async Task ResetAsync()
        {
            if (await confirmationDialog.ShowDialog())
            {
                await App.Names.ResetPairsAsync();
                Initialize();
            }
        }
    }
}