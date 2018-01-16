using System;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Core.DomainServices;
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
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(INetService),
                    typeof(WiFiService)));

            var ws = new WorkerService();

            //Act
            Action sut =
                () =>
                {
                    ws.Run(fixture.Create<Profiles>(), new Uri("https://www.google.com"), 1, 1, 1);
                };
            
            //Assert
            sut.ShouldNotThrow();
        }
    }
}