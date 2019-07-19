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
            RestoreCommand = new Command(
                async () => await Shell.Current.GoToAsync(
                    nameof(RestoreGirlNamePage)));
        }

        public override Command ShowRankedNamesCommand { get; }
        
        public override Command RestoreCommand { get; }
    }
}