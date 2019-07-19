using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class VoteNameViewModel : ViewModel
    {
        public VoteNameViewModel()
        {
        }

        public Command YesCommand { get; }

        public Command NoCommand { get; }
    }
}