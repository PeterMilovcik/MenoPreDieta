using System;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Annotations;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Extensions;
using MenoPreDieta.Models;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickNameViewModel : ViewModel
    {
        private NameModel first;
        private NameModel second;
        private int pairsCount;
        private int remainingPairsCount;
        private double progress;
        private readonly IConfirmationDialog confirmationDialog;
        private bool isBusy;

        public PickNameViewModel([NotNull] IConfirmationDialog confirmationDialog)
        {
            this.confirmationDialog = confirmationDialog ?? throw new ArgumentNullException(nameof(confirmationDialog));
            PickFirstNameCommand = new Command(async () => await PickFirstNameAsync(), () => !IsBusy);
            PickSecondNameCommand = new Command(async () => await PickSecondNameAsync(), () => !IsBusy);
            ResetCommand = new Command(async () =>
            {
                await ResetAsyncWithConfirmation();
            });
            ShowRankedNamesCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(RankedNamesPage)));
            MessagingCenter.Subscribe<RankedNamesViewModel>(
                this, "PairsUpdated", async sender => await InitializeAsync());
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

        public double Progress
        {
            get => progress;
            set
            {
                if (value.Equals(progress)) return;
                progress = value;
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
                OnPropertyChanged();
                PickFirstNameCommand.ChangeCanExecute();
                PickSecondNameCommand.ChangeCanExecute();
            }
        }

        public Command PickFirstNameCommand { get; }

        public Command PickSecondNameCommand { get; }

        public Command ResetCommand { get; }

        public Command ShowRankedNamesCommand { get; }

        public async Task InitializeAsync()
        {
            await App.Names.InitializePicksAsync();
            await UpdateAsync();
        }

        private async Task PickFirstNameAsync()
        {
            try
            {
                await PickNameAsync(App.Names.Picks.Selected.FirstNameId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task PickSecondNameAsync()
        {
            try
            {
                await PickNameAsync(App.Names.Picks.Selected.SecondNameId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task PickNameAsync(int nameId)
        {
            if (App.Names.Picks.Selected != null)
            {
                App.Names.Picks.Selected.PickedNameId = nameId;
                App.Names.Picks.Selected.IsProcessed = true;
                await App.Names.UpdateAsync(App.Names.Picks.Selected);
                await UpdateAsync();
            }
        }

        private async Task UpdateAsync()
        {
            if (App.Names.Picks.NotProcessed.Any())
            {
                App.Names.Picks.Selected = App.Names.Picks.NotProcessed.RandomItem();
                UpdateFirstAndSecondName();
            }
            else
            {
                await Shell.Current.Navigation.PopToRootAsync(false);
                ShowRankedNamesCommand.Execute(null);
            }

            UpdateStats();
        }

        private void UpdateFirstAndSecondName()
        {
            First = App.Names.Catalog.NameWithId(App.Names.Picks.Selected.FirstNameId).ToNameModel();
            Second = App.Names.Catalog.NameWithId(App.Names.Picks.Selected.SecondNameId).ToNameModel();
        }

        private void UpdateStats()
        {
            PairsCount = App.Names.Picks.Count;
            RemainingPairsCount = App.Names.Picks.NotProcessed.Count;
            Progress = PairsCount > 0 ? 1 - (double) RemainingPairsCount / PairsCount : 0;
        }

        private async Task ResetAsyncWithConfirmation()
        {
            try
            {
                if (await confirmationDialog.ShowDialog())
                {
                    await ResetAsync();
                    await Shell.Current.Navigation.PopToRootAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ResetAsync()
        {
            try
            {
                IsBusy = true;
                await App.Names.ResetAsync();
                await InitializeAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}