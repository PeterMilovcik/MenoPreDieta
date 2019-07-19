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
        private List<INameEntity> catalog;
        private List<INamePickEntity> pickPairs;
        private readonly Random random;
        private INamePickEntity namePick;
        private bool isBusy;
        private bool isEnabled;
        private readonly IConfirmationDialog confirmationDialog;
        private List<INamePickEntity> updateQueue;
        private List<INamePickEntity> deleteQueue;

        protected PickNameViewModel([NotNull] IConfirmationDialog confirmationDialog)
        {
            this.confirmationDialog = confirmationDialog ?? throw new ArgumentNullException(nameof(confirmationDialog));
            random = new Random();
            PickFirstNameCommand = new Command(async () => await PickFirstNameAsync());
            PickSecondNameCommand = new Command(async () => await PickSecondNameAsync());
            RemoveFirstNameCommand = new Command(async () => await RemoveFirstNameAsync());
            RemoveSecondNameCommand = new Command(async () => await RemoveSecondNameAsync());
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

        public abstract Command ShowRankedNamesCommand { get; }

        public abstract Command ResetCommand { get; }

        public abstract Command RestoreCommand { get; }

        public void Initialize()
        {
            try
            {
                IsBusy = true;
                catalog = App.Names.Catalog;
                NamesCount = catalog.Count;
                pickPairs = App.Names.Pairs;

                updateQueue = new List<INamePickEntity>();
                deleteQueue = new List<INamePickEntity>();
                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task PickFirstNameAsync() => 
            await PickNameAsync(namePick.FirstNameId);

        private async Task PickSecondNameAsync() => 
            await PickNameAsync(namePick.SecondNameId);

        private async Task PickNameAsync(int nameId)
        {
            if (namePick != null)
            {
                namePick.PickedNameId = nameId;
                namePick.IsNamePicked = true;
                updateQueue.Add(namePick);
                Update();
                await ProcessUpdateQueueAsync();
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
            var pairsToRemove =
                pickPairs.Where(pair => pair.FirstNameId == nameId ||
                                        pair.SecondNameId == nameId).ToList();
            pairsToRemove.ForEach(item =>
            {
                pickPairs.Remove(item);
                deleteQueue.Add(item);
            });
            Update();
            await ProcessDeleteQueueAsync();
            MessagingCenter.Send(this, "NameDeleted");
        }

        private async Task ProcessUpdateQueueAsync()
        {
            if (updateQueue.Any())
            {
                var item = updateQueue.First();
                await UpdateNamePickAsync(item);
                updateQueue.Remove(item);
                await ProcessUpdateQueueAsync();
            }
        }

        private async Task ProcessDeleteQueueAsync()
        {
            if (deleteQueue.Any())
            {
                var item = deleteQueue.First();
                await DeleteNamePicksAsync(item);
                deleteQueue.Remove(item);
                await ProcessDeleteQueueAsync();
            }
        }

        protected abstract Task UpdateNamePickAsync(INamePickEntity namePickEntity);

        protected abstract Task DeleteNamePicksAsync(INamePickEntity namePickEntity);

        private void Update()
        {
            var notPickedNamePairs = pickPairs.Where(pair => !pair.IsNamePicked).ToList();
            deleteQueue.ForEach(item => notPickedNamePairs.Remove(item));
            updateQueue.ForEach(item => notPickedNamePairs.Remove(item));
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

                namePick = GetRandomNamePick(
                    pairsToPick.Any()
                        ? pairsToPick
                        : notPickedNamePairs);
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
            var firstName = catalog.Single(name => name.Id == namePick.FirstNameId);
            First = new NameModel(firstName.Id, firstName.Value);
            var secondName = catalog.Single(name => name.Id == namePick.SecondNameId);
            Second = new NameModel(secondName.Id, secondName.Value);
        }

        private void UpdateStats()
        {
            PairsCount = pickPairs.Count;
            RemainingPairsCount = pickPairs.Count(pickPair => !pickPair.IsNamePicked);
            Accuracy = PairsCount > 0 ? 1 - (double) RemainingPairsCount / PairsCount : 0;
        }

        private INamePickEntity GetRandomNamePick(IReadOnlyList<INamePickEntity> list) => 
            list[random.Next(list.Count - 1)];

        protected async Task ResetAsync()
        {
            if (await confirmationDialog.ShowDialog())
            {
                await RecreateTableAsync();
                Initialize();
            }
        }

        protected abstract Task RecreateTableAsync();
    }
}