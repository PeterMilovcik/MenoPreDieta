using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickBoyNameViewModel : PickNameViewModel
    {
        public PickBoyNameViewModel(IConfirmationDialog confirmationDialog) 
            : base(confirmationDialog)
        {
            ShowRankedNamesCommand = new Command(
                async () => await Shell.Current.GoToAsync(
                    nameof(RankedBoyNamesPage)));
            ResetCommand = new Command(
                async () => await ResetAsync());
            RestoreCommand = new Command(
                async ()=> await Shell.Current.GoToAsync(nameof(RestoreBoyNamePage)));
        }

        public override Command ShowRankedNamesCommand { get; }

        public override Command ResetCommand { get; }

        public override Command RestoreCommand { get; }
        
        protected override Task RecreateTableAsync() => 
            App.Database.RecreateBoyNamesTableAsync();
    }
}