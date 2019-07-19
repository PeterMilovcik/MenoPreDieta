using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RestoreNameViewModel : ViewModel
    {
        private ObservableCollection<INameEntity> items;
        private INameEntity selectedItem;

        public RestoreNameViewModel()
        {
            RefreshCommand = new Command(Initialize);
            Random = new Random();
            RestoreNameCommand = new Command(async () => await RestoreNameAsync());
            MessagingCenter.Subscribe<PickNameViewModel>(
                this, "NameDeleted", sender => Initialize());
        }

        public void Initialize() => 
            Items = new ObservableCollection<INameEntity>(App.Names.Removed());

        protected Random Random { get; }

        public INameEntity SelectedItem
        {
            get => selectedItem;
            set
            {
                if (Equals(selectedItem, value)) return;
                selectedItem = value;
                if (selectedItem != null)
                {
                    RestoreNameCommand.Execute(value);
                }
                OnPropertyChanged();
            }
        }

        public Command RestoreNameCommand { get; }

        public ObservableCollection<INameEntity> Items
        {
            get => items;
            set
            {
                if (Equals(value, items)) return;
                items = value;
                OnPropertyChanged();
            }
        }

        public Command RefreshCommand { get; }

        private async Task RestoreNameAsync()
        {
            if (SelectedItem == null) return;
            var newPairs = new List<INamePickEntity>();
            foreach (var nameId in App.Names.Pairs.NameIds())
            {
                var firstName = SelectedItem;
                var secondName = App.Names.Catalog.NameWithId(nameId);
                if (Random.NextDouble() > 0.5)
                {
                    var temp = firstName;
                    firstName = secondName;
                    secondName = temp;
                }

                App.Names.Pairs.Add(App.Names.CreatePair(firstName.Id, secondName.Id));
            }
            await App.Names.AddToDatabase(newPairs);
            await Shell.Current.Navigation.PopAsync();
            MessagingCenter.Send(this, "PairsUpdated");
        }
    }
}