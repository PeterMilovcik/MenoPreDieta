using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickGirlNamePage : ContentPage
    {
        private readonly PickGirlNameViewModel viewModel;

        public PickGirlNamePage()
        {
            InitializeComponent();
            viewModel = new PickGirlNameViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadAsync();
        }
    }
}