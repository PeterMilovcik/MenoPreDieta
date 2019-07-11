using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RankedBoyNamesViewModel : RankedNamesViewModel
    {
        public RankedBoyNamesViewModel()
        {
            GenderColor = (Color) Application.Current.Resources["BlueLight"];
        }

        protected override Gender GetGender() => Gender.Boy;
    }
}
