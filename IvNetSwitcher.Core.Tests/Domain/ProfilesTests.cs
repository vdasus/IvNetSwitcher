using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Core.DomainServices;
using IvNetSwitcher.Core.Dto;
using Moq;
using Xunit;

namespace IvNetSwitcher.Core.Tests.Domain
{
    public class ProfilesTests
    {
        private readonly Fixture _fixture;

        public ProfilesTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(INetService),
                    typeof(WiFiService)));
        }

        [Fact]
        public void ctor_AllFieldsPresent()
        {
            //Arrange
            var fieldNames = typeof(Profiles).GetProperties()
                .Select(field => field.Name)
                .ToList();

            //Act
            //Assert
            fieldNames.Should().BeEquivalentTo(new List<string>
            {
                "Items"
            });
        }

        [Fact]
        public void GetCurrentProfile_Result()
        {
            //Arrange
            Profiles sutObjects = _fixture.Create<Profiles>();

            //Act
            var sut = sutObjects.GetCurrentProfile();
            var id = sutObjects.Items.FirstOrDefault(x => x.Active)?.Id;

            //Assert
            sut.Should().NotBeNull();
            sut.Id.Should().Be(id);
        }

        [Fact]
        public void CircularGetNextProfile_Result()
        {
            //Arrange
            Profiles sutObjects = new Profiles(new Mock<INetService>().Object, new List<ProfileDto>()
            {
                _fixture.Build<ProfileDto>().With(x => x.Id, 1).With(x => x.Active, false).With(x => x.Password,
                        @"aRUlqOEPBkRtVxYQFYktK/NwiQiQcbOdhFVbVdmyacxkGySWekJsM2QMwDvcPx82y5YL0xLFb/oyzVWJwLEKye/qu49t4pCJG0az0Q8BNrUaHL9dg4zHwWeVYf1+DaeJ")
                    .Create(),
                _fixture.Build<ProfileDto>().With(x => x.Id, 2).With(x => x.Active, true).With(x => x.Password,
                        @"aRUlqOEPBkRtVxYQFYktK/NwiQiQcbOdhFVbVdmyacxkGySWekJsM2QMwDvcPx82y5YL0xLFb/oyzVWJwLEKye/qu49t4pCJG0az0Q8BNrUaHL9dg4zHwWeVYf1+DaeJ")
                    .Create(),
                _fixture.Build<ProfileDto>().With(x => x.Id, 3).With(x => x.Active, false).With(x => x.Password,
                        @"aRUlqOEPBkRtVxYQFYktK/NwiQiQcbOdhFVbVdmyacxkGySWekJsM2QMwDvcPx82y5YL0xLFb/oyzVWJwLEKye/qu49t4pCJG0az0Q8BNrUaHL9dg4zHwWeVYf1+DaeJ")
                    .Create()
            }, "salt");

            //Act
            var sut0 = sutObjects.CircularGetNextProfile();
            var sut1 = sutObjects.CircularGetNextProfile();
            var sut2 = sutObjects.CircularGetNextProfile();
            var sut3 = sutObjects.CircularGetNextProfile();

            //Assert
            sut0.Id.Should().Be(3);
            sut1.Id.Should().Be(1);
            sut2.Id.Should().Be(2);
            sut3.Id.Should().Be(3);
        }
    }
}