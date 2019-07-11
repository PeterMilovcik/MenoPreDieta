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
            Routing.RegisterRoute(nameof(PickBoyNamePage), typeof(PickBoyNamePage));
            Routing.RegisterRoute(nameof(PickGirlNamePage), typeof(PickGirlNamePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}