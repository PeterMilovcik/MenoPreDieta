using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Dialogs;
using MenoPreDieta.Entities;
using MenoPreDieta.Views;
using Xamarin.Forms;

namespace MenoPreDieta.ViewModels
{
    public class PickBoyNameViewModel : PickNameViewModel<BoyNameEntity, BoyNamePickEntity>
    {
        public PickBoyNameViewModel(IConfirmationDialog confirmationDialog) : base(confirmationDialog)
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

        protected override Task InsertToDatabase(List<BoyNamePickEntity> namePicks) => 
            App.Database.InsertBoyNamePicksAsync(namePicks);

        protected override BoyNamePickEntity CreateNamePickEntity(int firstId, int secondId) =>
            new BoyNamePickEntity {FirstNameId = firstId, SecondNameId = secondId};

        protected override Task<List<BoyNameEntity>> GetNamesAsync() => 
            App.Database.GetBoyNamesAsync();

        protected override Task<List<BoyNamePickEntity>> GetNamePicksAsync() => 
            App.Database.GetBoyNamePicksAsync();

        protected override Task UpdateNamePickAsync(BoyNamePickEntity namePickEntity) => 
            App.Database.UpdateBoyNamePickAsync(namePickEntity);

        protected override Task DeleteNamePicksAsync(BoyNamePickEntity namePickEntity) => 
            App.Database.DeleteBoyNamePickAsync(namePickEntity);

        protected override Task RecreateTableAsync() => 
            App.Database.RecreateBoyNamesTableAsync();
    }
}