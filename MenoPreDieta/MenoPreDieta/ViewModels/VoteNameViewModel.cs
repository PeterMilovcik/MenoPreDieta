using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Extensions;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class VoteNameViewModel : ViewModel
    {
        private NameEntity name;
        private List<NameEntity> notProcessedNames;
        private double progress;
        private readonly IConfirmationDialog confirmationDialog;
        private bool isBusy;
        private readonly HashSet<NameEntity> undoQueue;

        public VoteNameViewModel(IConfirmationDialog confirmationDialog)
        {
            undoQueue = new HashSet<NameEntity>();
            this.confirmationDialog = confirmationDialog;
            YesCommand = new Command(async ()=> await YesAsync());
            NoCommand = new Command(async ()=> await NoAsync());
            ResetCommand = new Command(async () => await ResetWithConfirmationAsync());
            UndoCommand = new Command(async () => await UndoAsync());
        }

        public Command YesCommand { get; }

        public Command NoCommand { get; }

        public Command ResetCommand { get; }

        public Command UndoCommand { get; }

        public NameEntity Name
        {
            get => name;
            set
            {
                if (Equals(value, name)) return;
                name = value;
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
            }
        }

        public async Task LoadAsync()
        {
            notProcessedNames = App.Names.NotProcessed;
            Name = GetNewName();
            UpdatePercent();
            await CheckEmptyNameAsync();
        }

        private void UpdatePercent()
        {
            var totalCount = App.Names.Catalog.Count;
            var notProcessedCount = notProcessedNames.Count;
            if (totalCount > 0)
            {
                Progress = (totalCount - notProcessedCount) / (double)totalCount;
            }
            else
            {
                Progress = 0;
            }
        }

        private NameEntity GetNewName() => 
            Name = notProcessedNames.Any() 
                ? notProcessedNames.RandomItem() 
                : null;

        private async Task YesAsync()
        {
            try
            {
                if (Name == null) return;
                Name.IsLiked = true;
                await App.Names.ProcessAsync(Name);
                notProcessedNames.Remove(Name);
                undoQueue.Add(Name);
                Name = GetNewName();
                UpdatePercent();
                await CheckEmptyNameAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task NoAsync()
        {
            try
            {
                if (Name == null) return;
                Name.IsLiked = false;
                await App.Names.ProcessAsync(Name);
                notProcessedNames.Remove(Name);
                undoQueue.Add(Name);
                Name = GetNewName();
                UpdatePercent();
                await CheckEmptyNameAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task UndoAsync()
        {
            try
            {
                if (undoQueue.Any() == false) return;
                var last = undoQueue.Last();
                undoQueue.Remove(last);
                last.IsProcessed = false;
                await App.Names.UpdateAsync(last);
                notProcessedNames.Add(last);
                Name = last;
                UpdatePercent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task CheckEmptyNameAsync()
        {
            if (Name == null)
            {
                await Shell.Current.GoToAsync(nameof(PickNamePage));
            }
        }

        private async Task ResetWithConfirmationAsync()
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
                await LoadAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}