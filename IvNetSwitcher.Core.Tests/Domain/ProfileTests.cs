using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using IvNetSwitcher.Core.Domain;
using Xunit;

namespace IvNetSwitcher.Core.Tests.Domain
{
    public class ProfileTests
    {
        private const string SALT = "261A2220-C66E-496E-9DC0-5FF5174B7711";
        private const string PWD =
            @"IbP8X/dRF+c3YeCDtBg4d7ZzhQvhYDIZirJ9gAt/eoXPgH3QOWGWpeG65ZfrzPb3d9K2sY17bojnsYck3gaWYD+F+vq4jrVqvrh0fei3l5gWkGiBjP0xNXGw7Nm3ds/Y";

        private readonly IFixture _fixture;

        public ProfileTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customizations.Add(new SharedForTests.ProfileTests.PasswordArg<Profile>(PWD));
            _fixture.Customizations.Add(new SharedForTests.ProfileTests.SaltArg<Profile>(SALT));
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
                "Id", "Name", "User", "Password", "Domain", "Comment", "Active", "IsConnected"
            });
        }

        [Fact]
        public void ctor_AllFieldsInitialized()
        {
            //Arrange
            var sut = _fixture.Create<Profile>();
            
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