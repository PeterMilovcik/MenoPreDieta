using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankedGirlNamesPage : ContentPage
    {
        private readonly RankedGirlNamesViewModel viewModel;

        public RankedGirlNamesPage()
        {
            InitializeComponent();
            viewModel = new RankedGirlNamesViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadAsync();
        }
    }
}