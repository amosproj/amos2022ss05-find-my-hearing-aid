using FindMyBLEDevice.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyBLEDevice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothPage : ContentPage
    {

        BluetoothViewModel _viewModel;

        public BluetoothPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new BluetoothViewModel();
        }
    }
}