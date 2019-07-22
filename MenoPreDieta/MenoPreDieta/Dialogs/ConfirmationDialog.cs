using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MenoPreDieta.Dialogs
{
    public class ConfirmationDialog : IConfirmationDialog
    {
        public async Task<bool> ShowDialog() => 
            await Shell.Current.Navigation.NavigationStack.Last().DisplayAlert(
                "Otázka", 
                "Naozaj chceš začať odznova?", 
                "Áno", 
                "Nie");
    }
}