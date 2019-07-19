using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickGirlNameViewModel : PickNameViewModel
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

        protected override Task UpdateNamePickAsync(INamePickEntity namePickEntity) =>
            App.Database.UpdateGirlNamePickAsync(namePickEntity as GirlNamePickEntity);

        protected override Task DeleteNamePicksAsync(INamePickEntity namePickEntity) =>
            App.Database.DeleteGirlNamePickAsync(namePickEntity as GirlNamePickEntity);

        protected override Task RecreateTableAsync() =>
            App.Database.RecreateGirlNamesTableAsync();
    }
}