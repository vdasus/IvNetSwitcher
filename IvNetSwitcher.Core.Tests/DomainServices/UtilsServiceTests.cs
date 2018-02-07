using FluentAssertions;
using IvNetSwitcher.Core.DomainServices;
using Xunit;

namespace IvNetSwitcher.Core.Tests.DomainServices
{
    public class UtilsServiceTests
    {
        private const string APP_GUID = "730DE481-2077-4D59-AF65-543669121202";

        [Fact]
        public void EncryptDecrypt_Result()
        {
            var svc = new UtilsService();
            svc.SetSalt(APP_GUID);
            //Arrange
            var from = svc.GetEncryptedString("password");
            var to = svc.GetDecryptedString(from);
            
            //Act
            //Assert
            to.Should().Be("password");
        }
    }
}