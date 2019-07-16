using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class RankedBoyNamesViewModel : RankedNamesViewModel<BoyNameEntity, BoyNamePickEntity>
    {
        public RankedBoyNamesViewModel(IConfirmationDialog confirmationDialog) 
            : base(confirmationDialog)
        {
            GenderColor = (Color) Application.Current.Resources["BlueLight"];
            ResetCommand = new Command(
                async () =>
                {
                    await ResetAsync();
                    MessagingCenter.Send(this, "ResetBoyNamePicks");
                });
        }

        public override Command ResetCommand { get; }

        protected override Task<List<BoyNameEntity>> GetNamesAsync() => 
            App.Database.GetBoyNamesAsync();

        protected override Task<List<BoyNamePickEntity>> GetNamePicksAsync() => 
            App.Database.GetBoyNamePicksAsync();

        protected override Task PickNamesAsync() => 
            Shell.Current.GoToAsync(nameof(PickBoyNamePage));

        protected override Task RecreateTableAsync() =>
            App.Database.RecreateBoyNamesTableAsync();
    }
}
