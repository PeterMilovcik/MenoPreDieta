using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NameDescriptionPage : ContentPage
    {
        public NameDescriptionPage(NameDescriptionViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}