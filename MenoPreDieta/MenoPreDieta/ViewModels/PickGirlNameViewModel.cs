using MenoPreDieta.Entities;

namespace MenoPreDieta.ViewModels
{
    public class PickGirlNameViewModel : PickNameViewModel
    {
        protected override Gender GetGender() => Gender.Girl;
    }
}