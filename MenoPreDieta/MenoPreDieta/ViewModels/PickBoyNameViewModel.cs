using System.Linq;
using System.Threading.Tasks;
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

        public override async Task LoadAsync()
        {
            var names = await App.Database.GetNamesAsync();
            var boyNames = names.Where(name => name.Gender == Gender.Boy);
            NamesCount = boyNames.Count();
        }
    }
}