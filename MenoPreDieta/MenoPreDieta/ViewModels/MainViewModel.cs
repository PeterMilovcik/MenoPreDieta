using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            PickBoyNameCommand = new Command(
                async () =>
                {
                    App.PickBoyName();
                    await Shell.Current.GoToAsync(nameof(PickBoyNamePage));
                });
            PickGirlNameCommand = new Command(
                async () =>
                {
                    App.PickGirlName();
                    await Shell.Current.GoToAsync(nameof(PickGirlNamePage));
                });
        }

        public Command PickBoyNameCommand { get; }

        public Command PickGirlNameCommand { get; }
    }
}