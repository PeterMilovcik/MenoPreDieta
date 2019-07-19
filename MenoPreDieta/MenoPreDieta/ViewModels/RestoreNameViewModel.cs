using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public abstract class RestoreNameViewModel<TNameEntity, TNamePickEntity> : ViewModel
        where TNameEntity : INameEntity
        where TNamePickEntity : INamePickEntity
    {
        private ObservableCollection<TNameEntity> items;
        private bool isBusy;
        private Color genderColor;
        private TNameEntity selectedItem;

        protected RestoreNameViewModel()
        {
            RefreshCommand = new Command(async ()=> await LoadAsync());
            Random = new Random();
            MessagingCenter.Subscribe<PickNameViewModel<BoyNamePickEntity>>(
                this, "NameDeleted", async sender => await LoadAsync());
            MessagingCenter.Subscribe<PickNameViewModel<GirlNamePickEntity>>(
                this, "NameDeleted", async sender => await LoadAsync());
        }

        protected Random Random { get; }

        public TNameEntity SelectedItem
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

        public abstract Command RestoreNameCommand { get; }

        public ObservableCollection<TNameEntity> Items
        {
            get => items;
            set
            {
                if (Equals(value, items)) return;
                items = value;
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

        public Command RefreshCommand { get; }

        public async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                Names = await GetNamesAsync();
                NamePicks = await GetNamePicksAsync();
                var notRemovedNameIds = new HashSet<int>();
                NamePicks.ForEach(namePick =>
                {
                    notRemovedNameIds.Add(namePick.FirstNameId);
                    notRemovedNameIds.Add(namePick.SecondNameId);
                });
                Items = new ObservableCollection<TNameEntity>(
                    Names.Where(name => !notRemovedNameIds.Contains(name.Id)));
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected List<TNamePickEntity> NamePicks { get; private set; }

        protected List<TNameEntity> Names { get; private set; }

        protected abstract Task<List<TNameEntity>> GetNamesAsync();

        protected abstract Task<List<TNamePickEntity>> GetNamePicksAsync();
    }
}