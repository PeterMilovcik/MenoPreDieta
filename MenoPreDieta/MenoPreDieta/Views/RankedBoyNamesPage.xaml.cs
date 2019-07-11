using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankedBoyNamesPage : ContentPage
    {
        private readonly RankedBoyNamesViewModel viewModel;

        public RankedBoyNamesPage()
        {
            InitializeComponent();
            viewModel = new RankedBoyNamesViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadAsync();
        }
    }
}