using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RankedBoyNamesViewModel : RankedNamesViewModel<BoyNameEntity, BoyNamePickEntity>
    {
        public RankedBoyNamesViewModel()
        {
            GenderColor = (Color) Application.Current.Resources["BlueLight"];
        }

        protected override Task<List<BoyNameEntity>> GetNamesAsync() => 
            App.Database.GetBoyNamesAsync();

        protected override Task<List<BoyNamePickEntity>> GetNamePicksAsync() => 
            App.Database.GetBoyNamePicksAsync();
    }
}
