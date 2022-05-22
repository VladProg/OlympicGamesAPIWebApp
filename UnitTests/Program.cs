using OlympicGamesAPIWebApp;
using Xunit;

Console.WriteLine();
new UnitTests.ControllerTests().F();

namespace UnitTests
{
    public class ControllerTests
    {
        [Fact]
        public void F()
        {
            Assert.Equal(1, 2);
        }
    }
}