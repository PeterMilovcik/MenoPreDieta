using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestoreBoyNamePage : ContentPage
    {
        private readonly RestoreBoyNameViewModel viewModel;

        public RestoreBoyNamePage()
        {
            InitializeComponent();
            viewModel = new RestoreBoyNameViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadAsync();
        }
    }
}