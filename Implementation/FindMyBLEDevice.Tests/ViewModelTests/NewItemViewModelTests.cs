// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>


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
            nvg.Verify(mock => mock.GoToAsync("..", It.IsAny<bool>()), Times.Once);
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
            deviceStore.Setup(mock => mock.GetAllDevices()).Returns(Task.FromResult(new System.Collections.Generic.List<Models.BTDevice>()));

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
            nvg.Verify(mock => mock.GoToAsync("..", It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void OnSaveCommandUserLabel_Works()
        {
            // NewItemViewModel
            NewItemViewModel vm;
            var nvg = new Mock<INavigator>();

            string userLabel = "customLabel";
            string advertisedName = "advertisedName";
            var btDevice = new Models.BTDevice();
            btDevice.AdvertisedName = advertisedName;
            var deviceStore = new Mock<IDevicesStore>();
            deviceStore.SetupAllProperties();
            deviceStore.Object.SelectedDevice = btDevice;
            deviceStore.Setup(mock => mock.GetAllDevices()).Returns(Task.FromResult(new System.Collections.Generic.List<Models.BTDevice>()));

            int latitude = 10;
            int longitude = 10;
            var geolocation = new Mock<IGeolocation>();
            geolocation.Setup(mock => mock.GetCurrentLocation()).Returns(Task.FromResult(new Location()
            {
                Latitude = latitude,
                Longitude = longitude
            }));

            // act
            vm = new NewItemViewModel(nvg.Object, deviceStore.Object, geolocation.Object);
            vm.OnAppearing();
            vm.UserLabel = userLabel;
            vm.OnSave();

            // assert
            Assert.AreEqual(btDevice.UserLabel, userLabel);
            Assert.AreEqual(btDevice.LastGPSLatitude, latitude);
            Assert.AreEqual(btDevice.LastGPSLongitude, longitude);
            nvg.Verify(mock => mock.GoToAsync("..", It.IsAny<bool>()), Times.Once);
        }
    }
}