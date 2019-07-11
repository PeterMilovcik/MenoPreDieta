using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickGirlNameViewModel : PickNameViewModel
    {
        public PickGirlNameViewModel()
        {
            GenderColor = (Color)Application.Current.Resources["PinkLight"];
        }

        protected override Gender GetGender() => Gender.Girl;
    }
}