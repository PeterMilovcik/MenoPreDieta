using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public abstract class RankedNamesViewModel : ViewModel
    {
        private ObservableCollection<Model> items;
        private bool isBusy;
        private Color genderColor;

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

        protected abstract Gender GetGender();

        public async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                var names = await App.Database.GetNamesAsync();
                var genderNames = names.Where(name => name.Gender == GetGender()).ToList();
                var namePicks = await App.Database.GetNamePicksAsync();
                var genderNamePicks = namePicks.Where(np => np.Gender == GetGender() && np.IsNamePicked);
                var groupedNamePicks = genderNamePicks.GroupBy(gnp => gnp.PickedNameId);
                var orderedGroupedNamePicks = groupedNamePicks.OrderByDescending(gnp => gnp.Count());
                Items = new ObservableCollection<Model>();
                foreach (var pick in orderedGroupedNamePicks)
                {
                    var name = genderNames.Single(n => n.Id == pick.Key);
                    Items.Add(new Model(name.Id, name.Value, pick.Count()));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

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