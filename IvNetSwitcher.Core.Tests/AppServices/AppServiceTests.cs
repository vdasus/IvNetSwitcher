using System;
using FluentAssertions;
using IvNetSwitcher.Core.AppServices;
using IvNetSwitcher.Core.DomainServices;
using IvNetSwitcher.Core.Tests.Domain;
using Xunit;

namespace IvNetSwitcher.Core.Tests.AppServices
{
    public class AppServiceTests
    {
        [Fact]
        public void Run_Result()
        {
            //Arrange
            var ws = new AppService(new FakeService());

            //Act
            Action sut =
                () =>
                {
                    ws.LoadData(ProfilesFactory.CreateProfiles());
                    ws.Run(new Uri("https://www.google.com"), 1, 1, 1);
                };

            //Assert
            sut.ShouldNotThrow();
        }
    }
}