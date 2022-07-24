// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>


using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Geolocation;
using FindMyBLEDevice.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class NewItemViewModelTests
    {
        [TestMethod]
        public void Constructor_Works()
        {
            // arrange
            NewItemViewModel vm;

            // act
            vm = new NewItemViewModel(new Mock<INavigator>().Object, null, null);

            // assert
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.SaveCommand);
            Assert.IsNotNull(vm.CancelCommand);
        }

        [TestMethod]
        public void OnAppearing_Works()
        {
            // arrange
            NewItemViewModel vm;

            // act
            vm = new NewItemViewModel(new Mock<INavigator>().Object, null, null);
            vm.OnAppearing();

            // assert
            Assert.IsNull(vm.UserLabel);
        }

        [TestMethod]
        public void OnCancelCommand_Works()
        {
            // arrange
            NewItemViewModel vm;
            var nvg = new Mock<INavigator>();

            nvg.SetupGet(mock => mock.MapPage).Returns("MapPage");
            nvg.Setup(mock => mock.GoToAsync(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

            var deviceStore = new Mock<IDevicesStore>();
            deviceStore.Object.SelectedDevice = new Models.BTDevice();

            // act
            vm = new NewItemViewModel(nvg.Object, deviceStore.Object, null);
            vm.OnCancel();

            // assert
            // TODO: verify how to check for the right navigation
            // nvg.Verify(mock => mock.GoToAsync(It.Is<string>(pageName =>
            //   pageName == "MapPage"), It.IsAny<bool>()), Times.Once);
            Assert.IsNull(deviceStore.Object.SelectedDevice);
        }

        
        [TestMethod]
        public void OnSaveCommandUserLabelEmpty_Works()
        {
            // NewItemViewModel
            NewItemViewModel vm;
            var nvg = new Mock<INavigator>();

            string userLabel = "   ";
            string advertisedName = "advertisedName";
            var btDevice = new Models.BTDevice();
            btDevice.AdvertisedName = advertisedName;
            var deviceStore = new Mock<IDevicesStore>();
            deviceStore.SetupAllProperties();
            deviceStore.Object.SelectedDevice = btDevice;

            var geolocation = new Mock<IGeolocation>();
            geolocation.Setup(mock => mock.GetCurrentLocation()).Returns(Task.FromResult(new Location()
            {
                Latitude = 0,
                Longitude = 0
            }));

            // act
            vm = new NewItemViewModel(nvg.Object, deviceStore.Object, geolocation.Object);
            vm.OnAppearing();
            vm.UserLabel = userLabel;
            vm.OnSave();

            // assert
            Assert.AreEqual(btDevice.UserLabel, advertisedName);
        }
        /*
        [TestMethod]
        public void OnSaveCommandUserLabel_Works()
        {
            // NewItemViewModel
            NewItemViewModel vm;
            var nvg = new Mock<INavigator>();

            string userLabel = "MyNewName";
            string advertisedName = "advertisedName";
            var btDevice = new Models.BTDevice();
            btDevice.AdvertisedName = advertisedName;

            var deviceStore = new Mock<IDevicesStore>();
            deviceStore.Object.SelectedDevice = btDevice;

            // act
            vm = new NewItemViewModel(nvg.Object, null, null);
            vm.UserLabel = userLabel;
            vm.OnSave();

            // assert
            Assert.AreEqual(btDevice.UserLabel, userLabel);
        }
        */
    }
}