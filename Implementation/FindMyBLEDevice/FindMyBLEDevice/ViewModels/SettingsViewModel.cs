using FindMyBLEDevice.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Command SettingsCommand { get; }

        public SettingsViewModel()
        {
            SettingsCommand = new Command(OnSettingsClicked);
        }

        private async void OnSettingsClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}
