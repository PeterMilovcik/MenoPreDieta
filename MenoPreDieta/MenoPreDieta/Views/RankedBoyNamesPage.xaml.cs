using MenoPreDieta.Dialogs;
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
            viewModel = new RankedBoyNamesViewModel(new ConfirmationDialog(this));
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Initialize();
        }
    }
}