using System.Threading.Tasks;
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

        public override async Task LoadAsync()
        {
            
        }
    }
}