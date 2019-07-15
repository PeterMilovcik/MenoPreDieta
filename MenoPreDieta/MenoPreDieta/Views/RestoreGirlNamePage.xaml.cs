using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestoreGirlNamePage : ContentPage
    {
        private readonly RestoreGirlNameViewModel viewModel;

        public RestoreGirlNamePage()
        {
            InitializeComponent();
            viewModel = new RestoreGirlNameViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadAsync();
        }
    }
}