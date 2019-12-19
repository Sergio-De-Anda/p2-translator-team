using Microsoft.Extensions.Logging;
using P2Translator.Client.Controllers;
using Xunit;

namespace P2Translator.Testing.Client
{
  public class TestHomeController
  {
    private readonly ILogger<HomeController> logger = LoggerFactory.Create(o => o.SetMinimumLevel(LogLevel.Debug)).CreateLogger<HomeController>();

    [Fact]
    public void TestMessageBoard()
    {
      var home = new HomeController(logger);
      var messageboard = home.MessageBoard();
      Assert.NotNull(messageboard);
      
    }
  }
}