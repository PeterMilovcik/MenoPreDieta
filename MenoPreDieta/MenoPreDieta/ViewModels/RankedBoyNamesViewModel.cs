using MenoPreDieta.Dialogs;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RankedBoyNamesViewModel : RankedNamesViewModel
    {
        public RankedBoyNamesViewModel(IConfirmationDialog confirmationDialog) 
            : base(confirmationDialog)
        {
            ResetCommand = new Command(
                async () =>
                {
                    await ResetAsync();
                    MessagingCenter.Send(this, "ResetBoyNamePicks");
                });
        }

        public override Command ResetCommand { get; }
    }
}
