using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickBoyNamePage : ContentPage
    {
        private readonly PickBoyNameViewModel viewModel;

        public PickBoyNamePage()
        {
            InitializeComponent();
            viewModel = new PickBoyNameViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadAsync();
        }
    }
}