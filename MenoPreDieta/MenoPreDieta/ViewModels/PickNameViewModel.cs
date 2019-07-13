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
        private List<TNamePickEntity> updateQueue;
        private List<TNamePickEntity> deleteQueue;

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
                    await CreateNamePicks();
                }

                updateQueue = new List<TNamePickEntity>();
                deleteQueue = new List<TNamePickEntity>();
                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CreateNamePicks()
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

        protected abstract Task InsertToDatabase(List<TNamePickEntity> namePicks);

        protected abstract TNamePickEntity CreateNamePickEntity(int firstId, int secondId);

        protected abstract Task<List<TNameEntity>> GetNamesAsync();

        protected abstract Task<List<TNamePickEntity>> GetNamePicksAsync();

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

        protected abstract Task UpdateNamePickAsync(TNamePickEntity namePickEntity);

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
            deleteQueue.ForEach(item => notPickedNamePairs.Remove(item));
            updateQueue.ForEach(item => notPickedNamePairs.Remove(item));
            if (notPickedNamePairs.Any())
            {
                var pairsToPick = new List<TNamePickEntity>();
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
            var firstName = names.Single(name => name.Id == namePick.FirstNameId);
            First = new NameModel(firstName.Id, firstName.Value);
            var secondName = names.Single(name => name.Id == namePick.SecondNameId);
            Second = new NameModel(secondName.Id, secondName.Value);
        }

        private void UpdateStats()
        {
            PairsCount = pickPairs.Count;
            RemainingPairsCount = pickPairs.Count(pickPair => !pickPair.IsNamePicked);
            Accuracy = PairsCount > 0 ? 1 - (double) RemainingPairsCount / PairsCount : 0;
        }

        private TNamePickEntity GetRandomNamePick(IReadOnlyList<TNamePickEntity> list) => 
            list[random.Next(list.Count - 1)];

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