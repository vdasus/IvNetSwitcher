using System;
using FluentAssertions;
using IvNetSwitcher.Core.DomainServices;
using IvNetSwitcher.Core.Tests.Domain;
using Xunit;

namespace IvNetSwitcher.Core.Tests.DomainServices
{
    public class WorkerServiceTests
    {
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