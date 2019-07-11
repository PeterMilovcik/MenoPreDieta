using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RankedGirlNamesViewModel : RankedNamesViewModel
    {
        public RankedGirlNamesViewModel()
        {
            GenderColor = (Color) Application.Current.Resources["PinkLight"];
        }

        protected override Gender GetGender() => Gender.Girl;
    }
}