﻿using FindMyBLEDevice.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FindMyBLEDevice.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}