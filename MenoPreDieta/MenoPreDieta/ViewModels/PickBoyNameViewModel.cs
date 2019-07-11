using MenoPreDieta.Entities;

namespace MenoPreDieta.ViewModels
{
    public class PickBoyNameViewModel : PickNameViewModel
    {
        protected override Gender GetGender() => Gender.Boy;
    }
}