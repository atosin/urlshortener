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
        public void Shortener_Verify_Invalid_Urls_Return_Null_Sluges()
        {

            var mock = new Mock<IShortenerData>();

            mock.Setup(x => x.DoesSlugExist(It.IsAny<string>())).Returns(false);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedSlug = classUnderTest.GetSlugForUrl("www.yahoo.com");

            returnedSlug.Should().Be(null);

            returnedSlug = classUnderTest.GetSlugForUrl("https:www.yahoo.com");

            returnedSlug.Should().Be(null);

            returnedSlug = classUnderTest.GetSlugForUrl("https//:www.yahoo.com");

            returnedSlug.Should().Be(null);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Valid_Urls_Return_Valid_Sluges()
        {

            var mock = new Mock<IShortenerData>();

            mock.Setup(x => x.DoesSlugExist(It.IsAny<string>())).Returns(false);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedSlug = classUnderTest.GetSlugForUrl("https://www.yahoo.com");

            returnedSlug.Should().NotBe(null);

            returnedSlug = classUnderTest.GetSlugForUrl("https://www.yahoo.com?id=2");

            returnedSlug.Should().NotBe(null);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Different_Slug_Is_Provided_If_Slug_Exists()
        {

            var mock = new Mock<IShortenerData>();

            var existingSlug = "123";
            mock.Setup(x => x.DoesSlugExist(existingSlug)).Returns(true);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedSlug = classUnderTest.GetSlugForUrl("https://www.yahoo.com");

            returnedSlug.Should().NotBe(existingSlug);
        }


        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Same_Slug_Is_Provided_If_Slug_Exists_For_Url()
        {

            var mock = new Mock<IShortenerData>();

            var existingSlug = "123";
            var url = "https://www.yahoo.com";

            mock.Setup(x => x.GetSlugByUrl(url, out existingSlug)).Returns(true);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedSlug = classUnderTest.GetSlugForUrl(url);

            mock.Verify(x => x.UpdateLastAccessedDateForSlug(existingSlug), Times.Once);

            returnedSlug.Should().Be(existingSlug);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Slug_Is_Created_And_Saved_If_Slug_Does_Not_Exists_For_Url()
        {

            var mock = new Mock<IShortenerData>();

            var existingSlug = "123";
            var url = "https://www.yahoo.com";

            mock.Setup(x => x.GetSlugByUrl(url, out existingSlug)).Returns(false);

            mock.Setup(x => x.DoesSlugExist(It.IsAny<string>())).Returns(false);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedSlug = classUnderTest.GetSlugForUrl(url);

            mock.Verify(x => x.SaveShortenedUrl(returnedSlug, url), Times.Once);

            returnedSlug.Should().NotBe(null);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Url_Is_Null_If_Slug_Does_Not_Exist()
        {

            var mock = new Mock<IShortenerData>();

            var existingSlug = "123";

            mock.Setup(x => x.GetActualUrlBySlug(existingSlug)).Returns(default(string));

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedUrl = classUnderTest.GetActualUrl(existingSlug);

            returnedUrl.Should().Be(null);
        }

        [Fact]
        [Trait(TraitName, TraitValue)]
        public void Shortener_Verify_Url_Is_Not_Null_If_Slug_Does_Not_Exist()
        {

            var mock = new Mock<IShortenerData>();

            var existingSlug = "123";

            var url = "https://www.yahoo.com";

            mock.Setup(x => x.GetActualUrlBySlug(existingSlug)).Returns(url);

            IShortener classUnderTest = new Shortener(mock.Object);

            var returnedUrl = classUnderTest.GetActualUrl(existingSlug);

            returnedUrl.Should().Be(url);

            mock.Verify(x => x.UpdateLastAccessedDateForSlug(existingSlug), Times.Once);
        }
    }
}
