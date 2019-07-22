using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using MenoPreDieta.Extensions;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class VoteNameViewModel : ViewModel, ILoadableAsync
    {
        private NameEntity name;
        private List<NameEntity> notProcessedNames;
        private double percent;

        public VoteNameViewModel()
        { 
            YesCommand = new Command(async ()=> await YesAsync());
            NoCommand = new Command(async ()=> await NoAsync());
        }

        public Command YesCommand { get; }

        public Command NoCommand { get; }

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

        public double Percent
        {
            get => percent;
            set
            {
                if (value.Equals(percent)) return;
                percent = value;
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
                Percent = (totalCount - notProcessedCount) / (double)totalCount;
            }
            else
            {
                Percent = 0;
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

        private async Task CheckEmptyNameAsync()
        {
            if (Name == null)
            {
                await Shell.Current.GoToAsync(nameof(PickNamePage));
            }
        }
    }
}