using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RestoreBoyNameViewModel : RestoreNameViewModel<BoyNameEntity, BoyNamePickEntity>
    {
        public RestoreBoyNameViewModel()
        {
            GenderColor = (Color)Application.Current.Resources["BlueLight"];
            RestoreNameCommand = new Command(
                async () => await RestoreNameAsync());
        }

        public override Command RestoreNameCommand { get; }

        protected override Task<List<BoyNameEntity>> GetNamesAsync() => 
            App.Database.GetBoyNamesAsync();

        protected override Task<List<BoyNamePickEntity>> GetNamePicksAsync() =>
            App.Database.GetBoyNamePicksAsync();

        private async Task RestoreNameAsync()
        {
            if (SelectedItem == null) return;
            var notRemovedNameIds = new HashSet<int>();
            NamePicks.ForEach(namePick =>
            {
                notRemovedNameIds.Add(namePick.FirstNameId);
                notRemovedNameIds.Add(namePick.SecondNameId);
            });
            var list = notRemovedNameIds.ToList();
            var newPairs = new List<BoyNamePickEntity>();
            foreach (var notRemovedNameId in list)
            {
                var firstName = SelectedItem;
                var secondName = Names.Single(name=> name.Id == notRemovedNameId);
                if (Random.NextDouble() > 0.5)
                {
                    var temp = firstName;
                    firstName = secondName;
                    secondName = temp;
                }

                newPairs.Add(CreateNamePickEntity(firstName.Id, secondName.Id));
            }

            await InsertToDatabase(newPairs);

            await Shell.Current.Navigation.PopAsync();
        }

        private BoyNamePickEntity CreateNamePickEntity(int firstId, int secondId) =>
            new BoyNamePickEntity { FirstNameId = firstId, SecondNameId = secondId };

        private Task InsertToDatabase(List<BoyNamePickEntity> namePicks) =>
            App.Database.InsertBoyNamePicksAsync(namePicks);
    }
}