using System;
using FluentAssertions;
using IvNetSwitcher.Core.DomainServices;
using IvNetSwitcher.Core.Tests.Domain;
using Xunit;

namespace IvNetSwitcher.Core.Tests.DomainServices
{
    public class WorkerServiceTests
    {
        private const string SALT = "261A2220-C66E-496E-9DC0-5FF5174B7711";
        private const string PWD =
            @"IbP8X/dRF+c3YeCDtBg4d7ZzhQvhYDIZirJ9gAt/eoXPgH3QOWGWpeG65ZfrzPb3d9K2sY17bojnsYck3gaWYD+F+vq4jrVqvrh0fei3l5gWkGiBjP0xNXGw7Nm3ds/Y";

        [Fact]
        public void Run_Result()
        {
            //Arrange
            var ws = new WorkerService();

            //Act
            Action sut =
                () =>
                {
                    ws.Run(ProfilesFactory.CreateProfiles(), new Uri("https://www.google.com"), 1, 1, 1);
                };
            
            //Assert
            sut.ShouldNotThrow();
        }
    }
}