using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Annotations;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public abstract class RankedNamesViewModel<TNameEntity, TNamePickEntity> : ViewModel
        where TNameEntity : INameEntity
        where TNamePickEntity : INamePickEntity
    {
        private ObservableCollection<Model> items;
        private bool isBusy;
        protected IConfirmationDialog ConfirmationDialog { get; }

        protected RankedNamesViewModel([NotNull] IConfirmationDialog confirmationDialog)
        {
            ConfirmationDialog = confirmationDialog ?? throw new ArgumentNullException(nameof(confirmationDialog));
        }

        public ObservableCollection<Model> Items
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
        
        public async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                var names = await GetNamesAsync();
                var namePicks = (await GetNamePicksAsync()).Where(np => np.IsNamePicked);
                var groupedNamePicks = namePicks.GroupBy(gnp => gnp.PickedNameId);
                var orderedGroupedNamePicks = groupedNamePicks.OrderByDescending(gnp => gnp.Count());
                Items = new ObservableCollection<Model>();
                foreach (var pick in orderedGroupedNamePicks)
                {
                    var name = names.Single(n => n.Id == pick.Key);
                    Items.Add(new Model(name.Id, name.Value, pick.Count()));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected abstract Task<List<TNameEntity>> GetNamesAsync();

        protected abstract Task<List<TNamePickEntity>> GetNamePicksAsync();

        public abstract Command ResetCommand { get; }

        protected async Task ResetAsync()
        {
            if (await ConfirmationDialog.ShowDialog())
            {
                await RecreateTableAsync();
                await PickNamesAsync();
                await LoadAsync();
                await Shell.Current.Navigation.PopAsync();
            }
        }

        protected abstract Task PickNamesAsync();

        protected abstract Task RecreateTableAsync();

        public class Model
        {
            public int Id { get; }
            public string Name { get; }
            public int Count { get; }

            public Model(int id, string name, int count)
            {
                Id = id;
                Name = name;
                Count = count;
            }
        }
    }
}