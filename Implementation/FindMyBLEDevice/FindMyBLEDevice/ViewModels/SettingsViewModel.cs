using FindMyBLEDevice.Models;
using Xamarin.Essentials;

namespace FindMyBLEDevice.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel() { }
        public bool DisplayAllDevices
        {
            get => Preferences.Get(PreferenceNames.DisplayAllDevices, false);
            set
            {
                Preferences.Set(PreferenceNames.DisplayAllDevices, value);
                OnPropertyChanged(nameof(DisplayAllDevices));
            }
        }
    }
}
