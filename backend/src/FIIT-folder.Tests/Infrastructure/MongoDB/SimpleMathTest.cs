using NUnit.Framework;

namespace FIIT_folder.Tests.Infrastructure.MongoDB
{
    [TestFixture]
    public class SimpleMathTest
    {
        [Test]
        public void Two_Plus_Two_Equals_Four()
        {
            Assert.That(2 + 2, Is.EqualTo(4));
        }
    }
}