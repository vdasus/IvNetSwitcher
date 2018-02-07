using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using IvNetSwitcher.Core.Domain;
using Xunit;

namespace IvNetSwitcher.Core.Tests.Domain
{
    public class ProfilesTests
    {
        private readonly Profiles _sutObjects;

        public ProfilesTests()
        {
            _sutObjects = ProfilesFactory.CreateProfiles();
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
            //Act
            var sut = _sutObjects.GetCurrentProfile();
            var id = _sutObjects.Items.FirstOrDefault(x => x.Active)?.Id;

            //Assert
            sut.Should().NotBeNull();
            sut.Value.Id.Should().Be(id);
        }

        [Fact]
        public void CircularGetNextProfile_Result()
        {
            //Arrange
            //Act
            var sut0 = _sutObjects.CircularTakeNextProfile();
            var sut1 = _sutObjects.CircularTakeNextProfile();
            var sut2 = _sutObjects.CircularTakeNextProfile();
            var sut3 = _sutObjects.CircularTakeNextProfile();

            //Assert
            sut0.Value.Id.Should().Be(3);
            sut1.Value.Id.Should().Be(1);
            sut2.Value.Id.Should().Be(2);
            sut3.Value.Id.Should().Be(3);
        }
    }
}