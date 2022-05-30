using FindMyBLEDevice.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FindMyBLEDevice.Views
{
    public partial class ItemDetailPage : ContentPage
    {

        ItemDetailViewModel _viewModel;

        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.OnDisappearing();
        }


    }
}