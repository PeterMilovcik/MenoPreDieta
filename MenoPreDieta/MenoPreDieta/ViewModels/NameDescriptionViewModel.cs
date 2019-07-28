using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class NameDescriptionViewModel : ViewModel
    {
        public NameDescriptionViewModel(NameEntity name)
        {
            Name = name;
            CloseCommand = new Command(async () => await Shell.Current.Navigation.PopAsync());
        }

        public NameEntity Name { get; }

        public Command CloseCommand { get; }
    }
}
