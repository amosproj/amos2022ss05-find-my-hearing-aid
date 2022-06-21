using FindMyBLEDevice.ViewModels;
using Xamarin.Forms;

namespace FindMyBLEDevice.Views
{
    public partial class NewItemPage : ContentPage
    {
        NewItemViewModel _viewModel;

        public NewItemPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new NewItemViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}