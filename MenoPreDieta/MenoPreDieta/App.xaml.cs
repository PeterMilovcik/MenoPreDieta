using MenoPreDieta.Core;
using Xamarin.Forms;

namespace MenoPreDieta
{
    public partial class App : Application
    {
        private static Names boyNames;
        private static Names girlNames;

        public App()
        {
            Names = new UndefinedNames();
            InitializeComponent();
            MainPage = new AppShell();
        }

        public static Names Names { get; private set; }
        
        public static void PickBoyName()
        {
            Current.Resources["Primary"] = Current.Resources["BlueLight"];
            Current.Resources["PrimaryDark"] = Current.Resources["Blue"];
            Names = boyNames;
        }

        public static void PickGirlName()
        {
            Current.Resources["Primary"] = Current.Resources["PinkLight"];
            Current.Resources["PrimaryDark"] = Current.Resources["Pink"];
            Names = girlNames;
        }

        protected override async void OnStart()
        {
            boyNames = new BoyNames();
            girlNames = new GirlNames();
            await boyNames.InitializeAsync();
            await girlNames.InitializeAsync();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
