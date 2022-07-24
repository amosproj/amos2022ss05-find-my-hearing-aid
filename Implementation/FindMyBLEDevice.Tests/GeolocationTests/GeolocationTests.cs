// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Services.Geolocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FindMyBLEDevice.Tests.GeolocationTests
{
    [TestClass]
    public class GeolocationTests
    {
        [TestMethod]
        public async Task GetCurrentLocation_ReturnsCorrectLocation()
        {
            // arrange
            const int latitude = 0;
            const int longitude = 1;
            var locationAccess = new Mock<IGeolocationAccess>();
            locationAccess
                .Setup(mock => mock.GetLocationAsync(It.IsAny<GeolocationRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task<Location>.FromResult(new Location(latitude, longitude)));
            var location = new Services.Geolocation.Geolocation(locationAccess.Object);

            // act
            var result = await location.GetCurrentLocation();

            // assert
            Assert.AreEqual(latitude, result.Latitude);
            Assert.AreEqual(longitude, result.Longitude);
        }

        [TestMethod]
        public async Task GetCurrentLocation_ReturnsNullOnNoLocation()
        {
            // arrange
            var locationAccess = new Mock<IGeolocationAccess>();
            locationAccess
                .Setup(mock => mock.GetLocationAsync(It.IsAny<GeolocationRequest>(), It.IsAny<CancellationToken>()))
                .Returns<Task<Location>?>(null);
            var location = new Services.Geolocation.Geolocation(locationAccess.Object);

            // act
            var result = await location.GetCurrentLocation();

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCurrentLocation_CatchesFeatureNotSupportedException()
        {
            // arrange
            var locationAccess = new Mock<IGeolocationAccess>();
            locationAccess
                .Setup(mock => mock.GetLocationAsync(It.IsAny<GeolocationRequest>(), It.IsAny<CancellationToken>()))
                .Throws(new FeatureNotSupportedException());
            var location = new Services.Geolocation.Geolocation(locationAccess.Object);

            // act
            var result = await location.GetCurrentLocation();

            // assert
            // if the call would not catch the exception this test would fail
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCurrentLocation_CatchesFeatureNotEnabledException()
        {
            // arrange
            var locationAccess = new Mock<IGeolocationAccess>();
            locationAccess
                .Setup(mock => mock.GetLocationAsync(It.IsAny<GeolocationRequest>(), It.IsAny<CancellationToken>()))
                .Throws(new FeatureNotEnabledException());
            var location = new Services.Geolocation.Geolocation(locationAccess.Object);

            // act
            var result = await location.GetCurrentLocation();

            // assert
            // if the call would not catch the exception this test would fail
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCurrentLocation_CatchesPermissionException()
        {
            // arrange
            var locationAccess = new Mock<IGeolocationAccess>();
            locationAccess
                .Setup(mock => mock.GetLocationAsync(It.IsAny<GeolocationRequest>(), It.IsAny<CancellationToken>()))
                .Throws(new PermissionException("blabla"));
            var location = new Services.Geolocation.Geolocation(locationAccess.Object);

            // act
            var result = await location.GetCurrentLocation();

            // assert
            // if the call would not catch the exception this test would fail;
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCurrentLocation_CatchesAnyException()
        {
            // arrange
            var locationAccess = new Mock<IGeolocationAccess>();
            locationAccess
                .Setup(mock => mock.GetLocationAsync(It.IsAny<GeolocationRequest>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());
            var location = new Services.Geolocation.Geolocation(locationAccess.Object);

            // act
            var result = await location.GetCurrentLocation();

            // assert
            // if the call would not catch the exception this test would fail
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CancelLocationSearch_WithoutActiveSearchDoesNotBreak()
        {
            // arrange
            var location = new Services.Geolocation.Geolocation(new Mock<IGeolocationAccess>().Object);

            // act
            location.CancelLocationSearch();

            // assert
            // if no exception was thrown this test is successfull
        }
    }
}
