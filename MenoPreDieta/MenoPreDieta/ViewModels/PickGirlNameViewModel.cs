using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickGirlNameViewModel : PickNameViewModel<GirlNamePickEntity>
    {
        public PickGirlNameViewModel(IConfirmationDialog confirmationDialog) : base(confirmationDialog)
        {
            ShowRankedNamesCommand = new Command(
                async () => await Shell.Current.GoToAsync(
                    nameof(RankedGirlNamesPage)));
            ResetCommand = new Command(
                async () => await ResetAsync());
            RestoreCommand = new Command(
                async () => await Shell.Current.GoToAsync(nameof(RestoreGirlNamePage)));
        }

        public override Command ShowRankedNamesCommand { get; }

        public override Command ResetCommand { get; }

        public override Command RestoreCommand { get; }

        protected override Task InsertToDatabase(List<GirlNamePickEntity> namePicks) =>
            App.Database.InsertGirlNamePicksAsync(namePicks);

        protected override GirlNamePickEntity CreateNamePickEntity(int firstId, int secondId) =>
            new GirlNamePickEntity { FirstNameId = firstId, SecondNameId = secondId };
        
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