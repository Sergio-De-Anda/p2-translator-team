using P2Translator.WebApi.Models;
using Xunit;

namespace P2Translator.Testing.Web_Api
{
  public class SpeechUnitTest
  {
    [Fact]
    public void Test_EmptyMessage()
    {
      // Arrange
      Speech sch = new Speech();

      // Act out
      var actualResponse = sch.TextToSpeech("","English");
      
      // Assert
      Assert.False(actualResponse.Result);
    }

    [Fact]
    public void Test_EmptyLanguage()
    {
      // Arrange
      Speech sch = new Speech();

      // Act out
      var actualResponse = sch.TextToSpeech("Hello","");
      
      // Assert
      Assert.False(actualResponse.Result);
    }

      
  }
}