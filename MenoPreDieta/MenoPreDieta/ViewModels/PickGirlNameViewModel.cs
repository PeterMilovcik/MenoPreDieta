using MenoPreDieta.Entities;
using MenoPreDieta.Models;

namespace MenoPreDieta.ViewModels
{
    public class PickGirlNameViewModel : PickNameViewModel
    {
        public PickGirlNameViewModel()
        {
            First = new NameModel("TestFirstName");
            Second = new NameModel("TestSecondName");
        }

        protected override Gender GetGender() => Gender.Girl;
    }
}