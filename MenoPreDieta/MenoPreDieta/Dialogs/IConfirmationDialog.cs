using System.Threading.Tasks;

namespace MenoPreDieta.Dialogs
{
    public interface IConfirmationDialog
    {
        Task<bool> ShowDialog();
    }
}