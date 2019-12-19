using Microsoft.Extensions.Logging;
using P2Translator.Client.Controllers;
using P2Translator.Client.Models;
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
    [Fact]
    public void TestPrivacy()
    {
      var home = new HomeController(logger);
      var privacy = home.Privacy();
      Assert.NotNull(privacy);
      
    }
    [Fact]
    public void TestIndex()
    {
      var home = new HomeController(logger);
      var index = home.Index();
      Assert.NotNull(index);
      
    }
    [Fact]
    public void TestIndexx()
    {
      var home = new HomeController(logger);
      var indexx = home.Indexx();
      Assert.NotNull(indexx);
      
    }
    [Theory]
    [InlineData(1)]
    public void TestGetMessage(int id)
    {
      var home = new HomeController(logger);
      var message = home.MessageBoard(id);
      Assert.NotNull(message);
      
    }
    [Fact]
    public void TestMakeMessage()
    {
      MessageViewModel messageToTest = new MessageViewModel();
      var home = new HomeController(logger);
      var message = home.CreateMessage(messageToTest);
      Assert.NotNull(message);
    }
  }
}