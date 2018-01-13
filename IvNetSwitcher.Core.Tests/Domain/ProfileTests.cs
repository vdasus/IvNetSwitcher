using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using IvNetSwitcher.Core.Domain;
using Xunit;

namespace IvNetSwitcher.Core.Tests.Domain
{
    public class ProfileTests
    {
        private Fixture _fixture;

        public ProfileTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ctor_AllFieldsPresent()
        {
            //Arrange
            var fieldNames = typeof(Profile).GetProperties()
                .Select(field => field.Name)
                .ToList();

            //Act
            //Assert
            fieldNames.Should().BeEquivalentTo(new List<string>
            {
                "Id", "Name", "User", "Password", "Domain", "Comment", "Active"
            });
        }

        [Fact]
        public void ctor_AllFieldsInitialized()
        {
            //Arrange
            var sut = _fixture.Build<Profile>().With(x => x.Active, true).Create();
            
            //Act
            //Assert
            sut.Id.Should().BeOfType(typeof(int));
            sut.Name.Should().NotBeNullOrWhiteSpace();
            sut.User.Should().NotBeNullOrWhiteSpace();
            sut.Password.Should().NotBeNullOrWhiteSpace();
            sut.Domain.Should().NotBeNullOrWhiteSpace();
            sut.Comment.Should().NotBeNullOrWhiteSpace();
            sut.Active.Should().BeTrue();
        }
    }
}