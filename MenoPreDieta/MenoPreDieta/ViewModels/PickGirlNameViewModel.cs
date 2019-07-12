using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickGirlNameViewModel : PickNameViewModel<GirlNameEntity, GirlNamePickEntity>
    {
        public PickGirlNameViewModel(IConfirmationDialog confirmationDialog) : base(confirmationDialog)
        {
            GenderColor = (Color)Application.Current.Resources["PinkLight"];
            ShowRankedNamesCommand = new Command(
                async () => await Shell.Current.GoToAsync(
                    nameof(RankedGirlNamesPage)));
            ResetCommand = new Command(
                async () => await ResetAsync());
        }

        public override Command ShowRankedNamesCommand { get; }

        public override Command ResetCommand { get; }

        protected override Task InsertToDatabase(List<GirlNamePickEntity> namePicks) =>
            App.Database.InsertGirlNamePicksAsync(namePicks);

        protected override GirlNamePickEntity CreateNamePickEntity(int firstId, int secondId) =>
            new GirlNamePickEntity { FirstNameId = firstId, SecondNameId = secondId };

        protected override Task<List<GirlNameEntity>> GetNamesAsync() =>
            App.Database.GetGirlNamesAsync();

        protected override Task<List<GirlNamePickEntity>> GetNamePicksAsync() =>
            App.Database.GetGirlNamePicksAsync();

        protected override Task UpdateNamePickAsync(GirlNamePickEntity namePickEntity) =>
            App.Database.UpdateGirlNamePickAsync(namePickEntity);

        protected override Task DeleteNamePicksAsync(GirlNamePickEntity namePickEntity) =>
            App.Database.DeleteGirlNamePickAsync(namePickEntity);

        protected override Task RecreateTableAsync() =>
            App.Database.RecreateGirlNamesTableAsync();
    }
}