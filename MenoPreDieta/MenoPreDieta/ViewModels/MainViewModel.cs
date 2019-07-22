using System.Linq;
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
                    if (App.Names.NotProcessed.Any())
                    {
                        await Shell.Current.GoToAsync(nameof(VoteNamePage));
                    }
                    else
                    {
                        await Shell.Current.GoToAsync(nameof(PickNamePage));
                    }
                });
            PickGirlNameCommand = new Command(
                async () =>
                {
                    App.PickGirlName();
                    if (App.Names.NotProcessed.Any())
                    {
                        await Shell.Current.GoToAsync(nameof(VoteNamePage));
                    }
                    else
                    {
                        await Shell.Current.GoToAsync(nameof(PickNamePage));
                    }
                });
        }

        public Command PickBoyNameCommand { get; }

        public Command PickGirlNameCommand { get; }
    }
}