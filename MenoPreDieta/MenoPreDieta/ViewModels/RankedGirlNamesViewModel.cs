using MenoPreDieta.Dialogs;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RankedGirlNamesViewModel : RankedNamesViewModel
    {
        public RankedGirlNamesViewModel(IConfirmationDialog confirmationDialog)
            : base(confirmationDialog)
        {
            ResetCommand = new Command(async () =>
            {
                await ResetAsync();
                MessagingCenter.Send(this, "ResetGirlNamePicks");
            });
        }

        public override Command ResetCommand { get; }
    }
}