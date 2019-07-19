﻿using MenoPreDieta.Dialogs;
using MenoPreDieta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenoPreDieta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankedGirlNamesPage : ContentPage
    {
        private readonly RankedNamesViewModel viewModel;

        public RankedGirlNamesPage()
        {
            InitializeComponent();
            viewModel = new RankedNamesViewModel(new ConfirmationDialog(this));
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Initialize();
        }
    }
}