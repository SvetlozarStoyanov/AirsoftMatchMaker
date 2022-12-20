using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Tests.HelperClasses;

namespace AirsoftMatchMaker.Tests.UnitTests
{

    public class HtmlSanitizingServiceTests
    {
        private IHtmlSanitizingService htmlSanitizingService;
        [SetUp]
        public void Setup()
        {
            htmlSanitizingService = new HtmlSanitizingService();
        }


        [Test]
        public void Test_SanitizeStringProperty_RemovesHarmfulHtml()
        {
            var script = "<script>alert('danger')</script>message";
            var result = htmlSanitizingService.SanitizeStringProperty(script);

            Assert.That(result, Is.EqualTo("message"));
        }

        [Test]
        public void Test_SanitizeStringProperty_HandlesNulls()
        {
            string? script = null;
            var result = htmlSanitizingService.SanitizeStringProperty(script);

            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void Test_SanitizeObject_RemovesHarmfulHtml()
        {
            var model = new TestModel()
            {
                Name = "<script>alert('danger')</script>Name",
                Description = "<script>alert('danger')</script>Description",
                ImageUrl = "<script>alert('danger')</script>ImageUrl"
            };
            var resultModel = htmlSanitizingService.SanitizeObject<TestModel>(model);

            Assert.That(resultModel.Name, Is.EqualTo("Name"));
            Assert.That(resultModel.Description, Is.EqualTo("Description"));
            Assert.That(resultModel.ImageUrl, Is.EqualTo("ImageUrl"));
        }

        [Test]
        public void Test_SanitizeObject_HandlesNulls()
        {
            var model = new TestModel()
            {
                Name = null,
                Description = null,
                ImageUrl = null
            };
            var resultModel = htmlSanitizingService.SanitizeObject<TestModel>(model);

            Assert.That(resultModel.Name, Is.EqualTo(null));
            Assert.That(resultModel.Description, Is.EqualTo(null));
            Assert.That(resultModel.ImageUrl, Is.EqualTo(null));
        }


        [TearDown]
        public void TearDown()
        {

        }
    }

}
