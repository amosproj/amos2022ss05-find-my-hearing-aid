// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.ViewModels;
using FindMyBLEDevice.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class ItemDetailViewModelTests
    {
        [TestMethod]
        public void Constructor_Works()
        {
            // arrange
            ItemDetailViewModel vm;

            // act
            vm = new ItemDetailViewModel(null, null, null);

            // assert
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.StrengthButtonTapped);
            Assert.IsNotNull(vm.MapButtonTapped);
        }

        [TestMethod]
        public void StrengthButtonTapped_StopsPollingAndRedirects()
        {
            // arrange
            var nvg = new Mock<INavigator>();
            nvg.Setup(mock => mock.GoToAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var bt = new Mock<IBluetooth>();
            bt.Setup(mock => mock.StopRssiPolling());
            ItemDetailViewModel vm = new(nvg.Object, bt.Object, null);

            // act
            vm.StrengthButtonTapped.Execute(null);

            // assert
            bt.Verify(mock => mock.StopRssiPolling(), Times.Once);
            nvg.Verify(mock => mock.GoToAsync(It.Is<string>(pageName => 
                pageName == nameof(StrengthPage))), Times.Once);
        }

        [TestMethod]
        public void MapButtonTapped_StopsPollingAndRedirects()
        {
            // arrange
            var nvg = new Mock<INavigator>();
            nvg.Setup(mock => mock.GoToAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var bt = new Mock<IBluetooth>();
            bt.Setup(mock => mock.StopRssiPolling());
            ItemDetailViewModel vm = new(nvg.Object, bt.Object, null);

            // act
            vm.MapButtonTapped.Execute(null);

            // assert
            bt.Verify(mock => mock.StopRssiPolling(), Times.Once);
            nvg.Verify(mock => mock.GoToAsync(It.Is<string>(pageName =>
                pageName == nameof(MapPage))), Times.Once);
        }

        [TestMethod]
        public void OnAppearing_StartsPolling()
        {
            // arrange
            var bt = new Mock<IBluetooth>();
            bt.Setup(mock => mock.StartRssiPolling(It.IsAny<string>(), It.IsAny<Action<int,int>>(), It.IsAny<Action>(), It.IsAny<Action>()));
            var btd = new Models.BTDevice();
            btd.BT_GUID = "1234";
            var ds = new Mock<IDevicesStore>();
            ds.Setup(mock => mock.SelectedDevice).Returns(btd);
            ItemDetailViewModel vm = new(null, bt.Object, ds.Object);

            // act
            vm.OnAppearing();

            // assert
            bt.Verify(mock => mock.StartRssiPolling(
                It.Is<string>(guid => guid == btd.BT_GUID), 
                It.IsAny<Action<int, int>>(), It.IsAny<Action>(), It.IsAny<Action>()), 
                Times.Once);
        }

        [TestMethod]
        public void OnAppearing_UpdatesCurrentRssi()
        {
            // arrange
            var bt = new Mock<IBluetooth>();
            bt.Setup(mock => mock.StartRssiPolling(It.IsAny<string>(),It.IsAny<Action<int, int>>(), It.IsAny<Action>(), It.IsAny<Action>()))
                .Callback<string, Action<int, int>, Action, Action>((guid, update, connected, disconnected) => { update.Invoke(1, 0); });
            var btd = new Models.BTDevice();
            btd.BT_GUID = "1234";
            var ds = new Mock<IDevicesStore>();
            ds.Setup(mock => mock.SelectedDevice).Returns(btd);
            ItemDetailViewModel vm = new(null, bt.Object, ds.Object)
            {
                CurrentRssi = 0
            };
            Assert.AreEqual(0, vm.CurrentRssi);

            // act
            vm.OnAppearing();

            // assert
            Assert.AreEqual(1, vm.CurrentRssi);
        }

        [TestMethod]
        public void OnDisppearing_StopsPolling()
        {
            // arrange
            var bt = new Mock<IBluetooth>();
            bt.Setup(mock => mock.StopRssiPolling());
            ItemDetailViewModel vm = new(null, bt.Object, null);

            // act
            vm.OnDisappearing();

            // assert
            bt.Verify(mock => mock.StopRssiPolling(), Times.Once);
        }
    }
}