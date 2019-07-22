using MenoPreDieta.Dialogs;
using MenoPreDieta.ViewModels;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickNamePage
    {
        private readonly PickNameViewModel viewModel;

        public PickNamePage()
        {
            InitializeComponent();
            viewModel = new PickNameViewModel(new ConfirmationDialog());
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.InitializeAsync();
        }
    }
}