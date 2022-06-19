using FindMyBLEDevice.Models;
using Xamarin.Essentials;

namespace FindMyBLEDevice.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel() { }
        
        public bool DisplayNamelessDevices
        {
            get => Preferences.Get(PreferenceNames.DisplayNamelessDevices, false);
            set
            {
                Preferences.Set(PreferenceNames.DisplayNamelessDevices, value);
                OnPropertyChanged(nameof(DisplayNamelessDevices));
            }
        }
        public bool DisplayWeakDevices
        {
            get => Preferences.Get(PreferenceNames.DisplayWeakDevices, false);
            set
            {
                Preferences.Set(PreferenceNames.DisplayWeakDevices, value);
                OnPropertyChanged(nameof(DisplayWeakDevices));
            }
        }
    }
}
