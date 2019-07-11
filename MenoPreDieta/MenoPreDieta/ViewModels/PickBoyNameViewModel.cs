using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickBoyNameViewModel : PickNameViewModel
    {
        public PickBoyNameViewModel()
        {
            GenderColor = (Color)Application.Current.Resources["BlueLight"];
            ShowRankedNamesCommand = new Command(
                async () => await Shell.Current.GoToAsync(
                    nameof(RankedBoyNamesPage)));
            ResetCommand = new Command(
                async () => await ResetAsync());
        }

        public override Command ShowRankedNamesCommand { get; }

        public override Command ResetCommand { get; }

        protected override Gender GetGender() => Gender.Boy;
    }
}