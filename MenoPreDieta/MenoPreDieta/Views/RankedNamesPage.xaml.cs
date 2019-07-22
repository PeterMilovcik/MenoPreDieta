using MenoPreDieta.Dialogs;
using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankedNamesPage : ContentPage
    {
        private readonly RankedNamesViewModel viewModel;

        public RankedNamesPage()
        {
            InitializeComponent();
            viewModel = new RankedNamesViewModel(new ConfirmationDialog());
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Initialize();
        }
    }
}