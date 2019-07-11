using System.ComponentModel;
using System.Runtime.CompilerServices;
using MenoPreDieta.Annotations;
using MenoPreDieta.Models;

namespace MenoPreDieta.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}