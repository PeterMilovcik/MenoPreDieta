using System.Threading.Tasks;
using Xamarin.Forms;

namespace MenoPreDieta.Dialogs
{
    public class ConfirmationDialog : IConfirmationDialog
    {
        private readonly Page page;

        public ConfirmationDialog(Page page)
        {
            this.page = page;
        }

        public async Task<bool> ShowDialog() => 
            await page.DisplayAlert(
                "Otázka", 
                "Naozaj chcete začať odznova?", 
                "Áno", 
                "Nie");
    }
}