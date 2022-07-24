// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>

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
            vm = new AboutViewModel(new Mock<INavigator>().Object, null);

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
            vm = new AboutViewModel(nvg.Object, null);
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
            vm = new AboutViewModel(nvg.Object, null);
            vm.OpenStrengthPageCommand.Execute(null);

            // assert
            nvg.Verify(mock => mock.GoToAsync(It.Is<string>(pageName =>
               pageName == "StrengthPage"), It.IsAny<bool>()), Times.Once);

        }

    }
}