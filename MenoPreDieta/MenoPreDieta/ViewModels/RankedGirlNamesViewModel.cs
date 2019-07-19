using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RankedGirlNamesViewModel : RankedNamesViewModel<GirlNameEntity, GirlNamePickEntity>
    {
        public RankedGirlNamesViewModel(IConfirmationDialog confirmationDialog)
            : base(confirmationDialog)
        {
            ResetCommand = new Command(async () =>
            {
                await ResetAsync();
                MessagingCenter.Send(this, "ResetGirlNamePicks");
            });
        }

        public override Command ResetCommand { get; }

        protected override Task<List<GirlNameEntity>> GetNamesAsync() =>
            App.Database.GetGirlNamesAsync();

        protected override Task<List<GirlNamePickEntity>> GetNamePicksAsync() =>
            App.Database.GetGirlNamePicksAsync();

        protected override Task RecreateTableAsync() =>
            App.Database.RecreateGirlNamesTableAsync();

        protected override Task PickNamesAsync() =>
            Shell.Current.GoToAsync(nameof(PickGirlNamePage));
    }
}