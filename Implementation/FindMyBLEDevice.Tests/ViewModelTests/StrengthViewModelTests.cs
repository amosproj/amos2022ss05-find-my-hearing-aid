using FindMyBLEDevice.ViewModels;
using FindMyBLEDevice.XamarinAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

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
            var vm = new StrengthViewModel(disAcc.Object, null, null);

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
    }
}
