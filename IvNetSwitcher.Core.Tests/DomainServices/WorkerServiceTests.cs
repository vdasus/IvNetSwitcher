using System;
using AutoFixture;
using FluentAssertions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Core.DomainServices;
using Moq;
using Xunit;

namespace IvNetSwitcher.Core.Tests.DomainServices
{
    public class WorkerServiceTests
    {
        [Fact]
        public void Run_Result()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var wifisrv = new Mock<IServices>();
            var ws = new WorkerService(wifisrv.Object);

            //Act
            Action sut =
                () =>
                {
                    ws.Run(fixture.Create<Profiles>(), new Uri("https://www.google.com"), 1, 1, "test", 1);
                };
            
            //Assert
            sut.ShouldNotThrow();
        }
    }
}