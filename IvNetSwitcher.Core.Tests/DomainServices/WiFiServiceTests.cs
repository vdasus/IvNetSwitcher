#define DEVELPOPER_WITHOUT_WIFI

using System;
using FluentAssertions;
using IvNetSwitcher.Core.DomainServices;
using Xunit;

namespace IvNetSwitcher.Core.Tests.DomainServices
{
    public class WiFiServiceTests
    {
#if DEVELPOPER_WITHOUT_WIFI
        [Fact]
        public void ctor_Created()
        {
            //Arrange
            Action sut = () => { new WiFiService(); };
            //Act
            //Assert
            sut.Should().Throw<ApplicationException>();
        }
#else
        [Fact]
        public void ctor_Created()
        {
            //Arrange
            var sut = new WiFiServices();
            //Act
            //Assert
            sut.Should().NotBeNull();
        }
#endif
    }
}