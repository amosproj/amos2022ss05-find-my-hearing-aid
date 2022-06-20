using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Views;

namespace FindMyBLEDevice.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            
            OpenMapPageCommand = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(MapPage)}"));
            OpenStrengthPageCommand = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(StrengthPage)}"));
        }

        public ICommand OpenMapPageCommand { get; }

        public ICommand OpenStrengthPageCommand { get; }

        
    }
}
