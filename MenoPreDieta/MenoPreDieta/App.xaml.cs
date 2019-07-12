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
            await InitializeNames();
        }

        private static async Task InitializeNames()
        {
            await InitializeBoyNames();
            await InitializeGirlNames();
        }

        private static async Task InitializeBoyNames()
        {
            var names = await Database.GetBoyNamesAsync();
            if (!names.Any())
            {
                await Database.InsertBoyNamesAsync(new BoyNames());
            }
        }

        private static async Task InitializeGirlNames()
        {
            var names = await Database.GetGirlNamesAsync();
            if (!names.Any())
            {
                await Database.InsertGirlNamesAsync(new GirlNames());
            }
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
