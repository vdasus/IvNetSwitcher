

using FluentAssertions;
using IvNetSwitcher.Core.Shared;
using Xunit;

namespace IvNetSwitcher.Core.Tests.Shared
{
    public class UtilsTests
    {
        [Fact]
        public void EncryptDecrypt_Result()
        {
            //Arrange
            var from = Utils.GetEncryptedString("test string", "test salt");
            var to = Utils.GetDecryptedString(from, "test salt");
            
            //Act
            //Assert
            to.Should().Be("test string");
        }
    }
}