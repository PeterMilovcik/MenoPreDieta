using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestoreBoyNamePage : ContentPage
    {
        private readonly RestoreNameViewModel viewModel;

        public RestoreBoyNamePage()
        {
            InitializeComponent();
            viewModel = new RestoreNameViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Initialize();
        }
    }
}