using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RestoreGirlNameViewModel : RestoreNameViewModel<GirlNameEntity, GirlNamePickEntity>
    {
        public RestoreGirlNameViewModel()
        {
            GenderColor = (Color)Application.Current.Resources["PinkLight"];
            RestoreNameCommand = new Command(
                async ()=> await RestoreNameAsync());
        }

        public override Command RestoreNameCommand { get; }

        protected override Task<List<GirlNameEntity>> GetNamesAsync() =>
            App.Database.GetGirlNamesAsync();

        protected override Task<List<GirlNamePickEntity>> GetNamePicksAsync() =>
            App.Database.GetGirlNamePicksAsync();

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
            var newPairs = new List<GirlNamePickEntity>();
            foreach (var notRemovedNameId in list)
            {
                var firstName = SelectedItem;
                var secondName = Names.Single(name => name.Id == notRemovedNameId);
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

            MessagingCenter.Send(this, "RestoreGirlName");
        }

        private GirlNamePickEntity CreateNamePickEntity(int firstId, int secondId) =>
            new GirlNamePickEntity { FirstNameId = firstId, SecondNameId = secondId };

        private Task InsertToDatabase(List<GirlNamePickEntity> namePicks) =>
            App.Database.InsertGirlNamePicksAsync(namePicks);
    }
}