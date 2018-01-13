using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using IvNetSwitcher.Core.Domain;
using Xunit;

namespace IvNetSwitcher.Core.Tests.Domain
{
    public class ProfilesTests
    {
        private readonly Fixture _fixture;

        public ProfilesTests()
        {
            _fixture = new Fixture();
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
            Profiles sutObjects = new Profiles(new List<Profile>()
            {
                _fixture.Build<Profile>().With(x => x.Id, 1).With(x => x.Active, false).Create(),
                _fixture.Build<Profile>().With(x => x.Id, 2).With(x => x.Active, true).Create(),
                _fixture.Build<Profile>().With(x => x.Id, 3).With(x => x.Active, false).Create()
            });

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