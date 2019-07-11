using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickGirlNameViewModel : PickNameViewModel
    {
        public PickGirlNameViewModel()
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

        protected override Gender GetGender() => Gender.Girl;
    }
}