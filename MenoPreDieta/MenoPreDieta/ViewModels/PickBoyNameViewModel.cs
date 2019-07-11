using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickBoyNameViewModel : PickNameViewModel
    {
        public PickBoyNameViewModel()
        {
            GenderColor = (Color)Application.Current.Resources["Blue"];
        }

        protected override Gender GetGender() => Gender.Boy;
    }
}