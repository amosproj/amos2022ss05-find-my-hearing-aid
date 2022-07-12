// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>
// SPDX-FileCopyrightText: 2022 Dominik Pysch <domi.pysch@gmail.com>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.ViewModels;
using FindMyBLEDevice.XamarinAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class StrengthViewModelTests
    {
        [TestMethod]
        public void CircleSizesEquidistant()
        {
            // arrange
            var disAcc = new Mock<IDeviceDisplayAccess>();
            disAcc.SetupGet(mock => mock.Width).Returns(1000);
            disAcc.SetupGet(mock => mock.Density).Returns(10);
            var vm = new StrengthViewModel(disAcc.Object);

            // act
            var distances = new List<int>();
            for (int i = 0; i < vm.CircleSizes.Count - 1; i++)
            {
                distances.Add(Math.Abs(vm.CircleSizes[i] - vm.CircleSizes[i + 1]));
            }

            // assert
            int reference = distances[0];
            foreach (int dist in distances)
            {
                Assert.AreEqual(reference, dist, 2);
            }
        }

        [TestMethod]
        public void MaxRadiusSmallerThanScreenWidth()
        {
            // arrange
            var disAcc = new Mock<IDeviceDisplayAccess>();
            disAcc.SetupGet(mock => mock.Width).Returns(1000);
            disAcc.SetupGet(mock => mock.Density).Returns(10);

            // act
            var vm = new StrengthViewModel(disAcc.Object);

            // assert
            Assert.IsTrue(vm.CircleSizes.TrueForAll(s => s < (1000 / 10)));

        }


        [TestMethod]
        public void otherScaleToRadius_Test()
        {
            // arrange
            var disAcc = new Mock<IDeviceDisplayAccess>();
            disAcc.SetupGet(mock => mock.Width).Returns(1000);
            disAcc.SetupGet(mock => mock.Density).Returns(10);
            // therefore MaxRadiusSize = 90
            var vm = new StrengthViewModel(disAcc.Object);


            // act
            MethodInfo methodInfo = typeof(StrengthViewModel).GetMethod("otherScaleToRadius", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { 0, 10, 6 };
            int a = (int) methodInfo.Invoke(vm, parameters);
            object[] parameters2 = { 20, 200, 100 };
            int b = (int)methodInfo.Invoke(vm, parameters2);

            // assert
            // 36 = 60 * (6/10)
            Assert.AreEqual(30 + 36, a);
            // 27 = 60 * (80/180)
            Assert.AreEqual(30 + 27, b, 1);

        }

        [TestMethod]
        public void rssiToMeter_SensibleResults()
        {
            // arrange
            var disAcc = new Mock<IDeviceDisplayAccess>();
            disAcc.SetupGet(mock => mock.Width).Returns(1000);
            disAcc.SetupGet(mock => mock.Density).Returns(10);
            var vm = new StrengthViewModel(disAcc.Object);

            // act
            MethodInfo methodInfo = typeof(StrengthViewModel).GetMethod("rssiToMeter", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { -30, Constants.TxPowerDefault, Constants.RssiEnvironmentalDefault };
            double a = (double) methodInfo.Invoke(vm, parameters);
            object[] parameters2 = { -80, Constants.TxPowerDefault, Constants.RssiEnvironmentalDefault };
            double b = (double) methodInfo.Invoke(vm, parameters2);

            // assert
            Assert.IsTrue(a > 0 && a < 10);            
            Assert.IsTrue(b > 0 && b < 100);

        }

    }
}
