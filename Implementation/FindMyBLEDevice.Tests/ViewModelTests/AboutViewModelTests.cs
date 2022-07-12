// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Dominik Pysch <domi.pysch@gmail.com>

using FindMyBLEDevice.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class AboutViewModelTests
    {
        [TestMethod]
        public void Constructor_Works()
        {
            // arrange
            AboutViewModel vm;

            // act
            vm = new AboutViewModel(new Mock<INavigator>().Object);

            // assert
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.OpenMapPageCommand);
            Assert.IsNotNull(vm.OpenStrengthPageCommand);
        }

        [TestMethod]
        public void OpenMapPageCommand_Works()
        {
            // arrange
            AboutViewModel vm;
            var nvg = new Mock<INavigator>();

            nvg.SetupGet(mock => mock.MapPage).Returns("MapPage");
            nvg.Setup(mock => mock.GoToAsync(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.CompletedTask);


            // act
            vm = new AboutViewModel(nvg.Object);
            vm.OpenMapPageCommand.Execute(null);

            // assert
            nvg.Verify(mock => mock.GoToAsync(It.Is<string>(pageName =>
               pageName == "MapPage"), It.IsAny<bool>()), Times.Once);

        }

        [TestMethod]
        public void OpenStrengthPageCommand_Works()
        {
            // arrange
            AboutViewModel vm;
            var nvg = new Mock<INavigator>();

            nvg.SetupGet(mock => mock.StrengthPage).Returns("StrengthPage");
            nvg.Setup(mock => mock.GoToAsync(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

            // act
            vm = new AboutViewModel(nvg.Object);
            vm.OpenStrengthPageCommand.Execute(null);

            // assert
            nvg.Verify(mock => mock.GoToAsync(It.Is<string>(pageName =>
               pageName == "StrengthPage"), It.IsAny<bool>()), Times.Once);

        }

    }
}