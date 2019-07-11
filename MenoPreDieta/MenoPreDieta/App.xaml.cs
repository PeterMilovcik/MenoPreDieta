using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Data;
using Xamarin.Forms;

namespace MenoPreDieta
{
    public partial class App : Application
    {
        private static Database database;

        public static Database Database =>
            database ?? (database =
                new Database(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "database.db3")));

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            var names = await Database.GetNamesAsync();
            if (!names.Any())
            {
                await AddNamesToDatabaseAsync();
            }
        }

        private async Task AddNamesToDatabaseAsync()
        {
            
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
