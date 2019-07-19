using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Data;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace MenoPreDieta.Core
{
    public abstract class Names
    {
        public abstract Color Color { get; }
        public abstract Task InitializeAsync();
    }

    public class UndefinedNames : Names
    {
        public UndefinedNames()
        {
            Color = Color.White;
        }

        public override Color Color { get; }

        public override Task InitializeAsync() => Task.CompletedTask;
    }

    public class BoyNames : Names
    {
        public override Color Color => (Color)Application.Current.Resources["BlueLight"];

        public override async Task InitializeAsync()
        {
            var names = await App.Database.GetBoyNamesAsync();
            if (!names.Any())
            {
                await App.Database.InsertBoyNamesAsync(new BoyNamesCatalog());
            }
        }
    }

    public class GirlNames : Names
    {
        public override Color Color => (Color)Application.Current.Resources["PinkLight"];

        public override async Task InitializeAsync()
        {
            var names = await App.Database.GetGirlNamesAsync();
            if (!names.Any())
            {
                await App.Database.InsertGirlNamesAsync(new GirlNamesCatalog());
            }
        }
    }
}
