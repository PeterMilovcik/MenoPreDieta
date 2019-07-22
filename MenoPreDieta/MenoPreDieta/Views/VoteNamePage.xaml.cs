using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoteNamePage : ContentPage
    {
        public VoteNamePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ILoadableAsync loadable)
            {
                await loadable.LoadAsync();
            }
        }
    }
}