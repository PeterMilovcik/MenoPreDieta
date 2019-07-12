using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RankedGirlNamesViewModel : RankedNamesViewModel<GirlNameEntity, GirlNamePickEntity>
    {
        public RankedGirlNamesViewModel()
        {
            GenderColor = (Color) Application.Current.Resources["PinkLight"];
        }

        protected override Task<List<GirlNameEntity>> GetNamesAsync() =>
            App.Database.GetGirlNamesAsync();

        protected override Task<List<GirlNamePickEntity>> GetNamePicksAsync() =>
            App.Database.GetGirlNamePicksAsync();
    }
}