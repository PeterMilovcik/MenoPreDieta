using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickBoyNameViewModel : PickNameViewModel
    {
        public PickBoyNameViewModel(IConfirmationDialog confirmationDialog) 
            : base(confirmationDialog)
        {
            ShowRankedNamesCommand = new Command(
                async () => await Shell.Current.GoToAsync(
                    nameof(RankedBoyNamesPage)));
            ResetCommand = new Command(
                async () => await ResetAsync());
            RestoreCommand = new Command(
                async ()=> await Shell.Current.GoToAsync(nameof(RestoreBoyNamePage)));
        }

        public override Command ShowRankedNamesCommand { get; }

        public override Command ResetCommand { get; }

        public override Command RestoreCommand { get; }

        protected override Task InsertToDatabase(List<INamePickEntity> namePicks) => 
            App.Database.InsertBoyNamePicksAsync(namePicks.OfType<BoyNamePickEntity>());

        protected override INamePickEntity CreateNamePickEntity(int firstId, int secondId) =>
            new BoyNamePickEntity {FirstNameId = firstId, SecondNameId = secondId};
        
        protected override Task UpdateNamePickAsync(INamePickEntity namePickEntity) => 
            App.Database.UpdateBoyNamePickAsync(namePickEntity as BoyNamePickEntity);

        protected override Task DeleteNamePicksAsync(INamePickEntity namePickEntity) => 
            App.Database.DeleteBoyNamePickAsync(namePickEntity as BoyNamePickEntity);

        protected override Task RecreateTableAsync() => 
            App.Database.RecreateBoyNamesTableAsync();
    }
}