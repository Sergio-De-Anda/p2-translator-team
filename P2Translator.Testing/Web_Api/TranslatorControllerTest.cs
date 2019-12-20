using System.Collections.Generic;
using P2Translator.Data;
using P2Translator.Data.Models;
using P2Translator.WebApi.Controllers;
using Xunit;

namespace P2Translator.Testing.Web_Api
{
  public class TranslatorControllerTest
  {
    private readonly P2TranslatorDbContext _db;
    [Fact]
    public void TestGetMessages()
    {
      var home = new TranslatorController(_db);
      var result = home.GetMessages();
      Assert.NotNull(result);
      
    }
    [Theory]
    [InlineData(1)]
    public void TestGetMessage(int id)
    {
      var home = new TranslatorController(_db);
      var message = home.GetMessage(id);
      Assert.NotNull(message);
      
    }
    [Theory]
    [InlineData("Spanish")]
    public void TestGetMessagesTranslated(string language)
    {
      var home = new TranslatorController(_db);
      var messages = home.GetMessages(language);
      Assert.NotNull(messages);
      
    }

    [Fact]
    public void TestPost()
    {
      Message messageToTest = new Message();
      var home = new TranslatorController(_db);
      var result = home.Post(messageToTest);
      Assert.NotNull(result);
    }

    [Fact]
    public void TestGetLanguages()
    {
      var home = new TranslatorController(_db);
      var result = home.GetLanguages();
      Assert.NotNull(result);
    } 
  }
}