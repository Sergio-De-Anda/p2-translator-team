using P2Translator.WebApi.Models;
using Xunit;

namespace P2Translator.Testing.Web_Api
{
  public class TranslatorUnitTest
  {
    [Fact]
    public void Test_ValidMessageResponse()
    {
      // Arrange
      string messageRequest = "valid message";
      string messageResponse = "mensaje válido";

      // Act out
      Translator actualResponse = new Translator();
      
      // Assert
      Assert.True(actualResponse.Translate(messageRequest, "spanish").Result.Equals(messageResponse));
    }

    [Fact]
    public void Test_EmptyMessage()
    {
      // Arrange
      string messageRequest = "";

      // Act out
      Translator actualResponse = new Translator();

      // Assert
      Assert.True(string.IsNullOrEmpty(actualResponse.Translate(messageRequest, "Spanish").Result));
    }

    [Fact]
    public void Test_NonexistingLanguage()
    {
      // Arrange
      string messageRequest = "valid message";
      string dummyLanguage = "dummy language";

      // Act out
      Translator actualResponse = new Translator();

      // Assert
      Assert.True(string.IsNullOrEmpty(actualResponse.Translate(messageRequest, dummyLanguage).Result));
    }

    [Fact]
    public void Test_EmptyLanguage()
    {
      // Arrange
      string messageRequest = "valid message";
      string emptyLanguage = "";

      // Act out
      Translator actualResponse = new Translator();

      // Assert
      Assert.True(string.IsNullOrEmpty(actualResponse.Translate(messageRequest, emptyLanguage).Result));
    }

  }
}