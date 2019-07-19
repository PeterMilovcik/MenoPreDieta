using MenoPreDieta.Dialogs;
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
            RestoreCommand = new Command(
                async ()=> await Shell.Current.GoToAsync(
                    nameof(RestoreBoyNamePage)));
        }

        public override Command ShowRankedNamesCommand { get; }

        public override Command RestoreCommand { get; }
    }
}