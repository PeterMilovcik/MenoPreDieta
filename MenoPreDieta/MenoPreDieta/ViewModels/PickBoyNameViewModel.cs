using MenoPreDieta.Entities;
using MenoPreDieta.Models;

namespace MenoPreDieta.ViewModels
{
    public class PickBoyNameViewModel : PickNameViewModel
    {
        public PickBoyNameViewModel()
        {
            First = new NameModel("TestFirstName");
            Second = new NameModel("TestSecondName");
        }

        protected override Gender GetGender() => Gender.Boy;
    }
}