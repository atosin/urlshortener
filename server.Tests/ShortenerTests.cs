using FluentAssertions;
using Moq;
using server.Business;
using server.Data;
using Xunit;

namespace server.Tests
{
    public class ShortenerTests
    {
        private const string TraitName = "Business";
        private const string TraitValue = " Shortener";

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Invalid_Urls_Return_Null_Hashes()
        {

            var mock = new Mock<IShortenerData>();

            mock.Setup(x => x.DoesHashExist(It.IsAny<string>())).Returns(false);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedHash = classUnderTest.GetHashFromUrl("www.yahoo.com");

            returnedHash.Should().Be(null);

            returnedHash = classUnderTest.GetHashFromUrl("https:www.yahoo.com");

            returnedHash.Should().Be(null);

            returnedHash = classUnderTest.GetHashFromUrl("https//:www.yahoo.com");

            returnedHash.Should().Be(null);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Valid_Urls_Return_Valid_Hashes()
        {

            var mock = new Mock<IShortenerData>();

            mock.Setup(x => x.DoesHashExist(It.IsAny<string>())).Returns(false);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedHash = classUnderTest.GetHashFromUrl("https://www.yahoo.com");

            returnedHash.Should().NotBe(null);

            returnedHash = classUnderTest.GetHashFromUrl("https://www.yahoo.com?id=2");

            returnedHash.Should().NotBe(null);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Different_Hash_Is_Provided_If_Hash_Exists()
        {

            var mock = new Mock<IShortenerData>();

            var existingHash = "123";
            mock.Setup(x => x.DoesHashExist(existingHash)).Returns(true);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedHash = classUnderTest.GetHashFromUrl("https://www.yahoo.com");

            returnedHash.Should().NotBe(existingHash);
        }


        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Same_Hash_Is_Provided_If_Hash_Exists_For_Url()
        {

            var mock = new Mock<IShortenerData>();

            var existingHash = "123";
            var url = "https://www.yahoo.com";

            mock.Setup(x => x.GetHashByUrl(url, out existingHash)).Returns(true);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedHash = classUnderTest.GetHashFromUrl(url);

            mock.Verify(x => x.UpdateLastAccessedDateForHash(existingHash), Times.Once);

            returnedHash.Should().Be(existingHash);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Hash_Is_Created_And_Saved_If_Hash_Does_Not_Exists_For_Url()
        {

            var mock = new Mock<IShortenerData>();

            var existingHash = "123";
            var url = "https://www.yahoo.com";

            mock.Setup(x => x.GetHashByUrl(url, out existingHash)).Returns(false);

            mock.Setup(x => x.DoesHashExist(It.IsAny<string>())).Returns(false);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedHash = classUnderTest.GetHashFromUrl(url);

            mock.Verify(x => x.SaveShortenedUrl(returnedHash, url), Times.Once);

            returnedHash.Should().NotBe(null);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Url_Is_Null_If_Hash_Does_Not_Exist()
        {

            var mock = new Mock<IShortenerData>();

            var existingHash = "123";

            mock.Setup(x => x.GetActualUrlByHash(existingHash)).Returns(default(string));

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedUrl= classUnderTest.GetActualUrl(existingHash);

            returnedUrl.Should().Be(null);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Url_Is_Not_Null_If_Hash_Does_Not_Exist()
        {

            var mock = new Mock<IShortenerData>();

            var existingHash = "123";

            var url = "https://www.yahoo.com";

            mock.Setup(x => x.GetActualUrlByHash(existingHash)).Returns(url);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedUrl = classUnderTest.GetActualUrl(existingHash);

            returnedUrl.Should().Be(url);

            mock.Verify(x => x.UpdateLastAccessedDateForHash(existingHash), Times.Once);
        }
    }

}
