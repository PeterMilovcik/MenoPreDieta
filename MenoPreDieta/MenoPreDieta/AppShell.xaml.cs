using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(VoteNamePage), typeof(VoteNamePage));
            Routing.RegisterRoute(nameof(PickNamePage), typeof(PickNamePage));
            Routing.RegisterRoute(nameof(RankedNamesPage), typeof(RankedNamesPage));
        }
    }
}