using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace P2Translator.WebApi.Models
{
  public class Speech
  {
    private string subscriptionKey = "5dbb936323894a3abead86291b52d1b4";
    private string tokenFetchUri = "https://centralus.api.cognitive.microsoft.com/sts/v1.0/issuetoken";

    public Speech()
    {
      if (string.IsNullOrWhiteSpace(tokenFetchUri))
      {
          throw new ArgumentNullException(nameof(tokenFetchUri));
      }
      if (string.IsNullOrWhiteSpace(subscriptionKey))
      {
          throw new ArgumentNullException(nameof(subscriptionKey));
      }
    }
    private async Task<string> FetchTokenAsync()
    {
      using (HttpClient client = new HttpClient())
      {
          client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.subscriptionKey);
          UriBuilder uriBuilder = new UriBuilder(this.tokenFetchUri);

          HttpResponseMessage result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null).ConfigureAwait(false);
          return await result.Content.ReadAsStringAsync().ConfigureAwait(false);
      }
    }

    public async Task<bool> TextToSpeech(string text, string language)
    {
      if(string.IsNullOrEmpty(text) || string.IsNullOrEmpty(language))
      {
        return false;
      }
      // string accessToken;

      // try
      // {
      //     accessToken = await FetchTokenAsync().ConfigureAwait(false);
      //     // Console.WriteLine("Successfully obtained an access token. \n");
      // }
      // catch (Exception ex)
      // {
      //     // Console.WriteLine("Failed to obtain an access token.");
      //     Console.WriteLine(ex.ToString());
      //     Console.WriteLine(ex.Message);
      // }

      string host = "https://centralus.tts.speech.microsoft.com/cognitiveservices/v1";

      // Create SSML document.
      XDocument body = new XDocument(
                        new XElement("speak",
                          new XAttribute("version", "1.0"),
                          new XAttribute(XNamespace.Xml + "lang", "en-US"),
                            new XElement("voice",
                                new XAttribute(XNamespace.Xml + "lang", "en-US"),
                                new XAttribute(XNamespace.Xml + "gender", "Female"),
                                new XAttribute("name", "en-US-Jessa24kRUS"),
                                  text)));

      using (HttpClient client = new HttpClient())
      {
        using (HttpRequestMessage request = new HttpRequestMessage())
        {
          // Set the HTTP method
          request.Method = HttpMethod.Post;
          // Construct the URI
          request.RequestUri = new Uri(host);
          // Set the content type header
          request.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/ssml+xml");
          // Set additional header, such as Authorization and User-Agent
          try{
            request.Headers.Add("Authorization", "Bearer " + await FetchTokenAsync().ConfigureAwait(false));
          }
          catch(Exception ex)
          {
            Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.Message);
            return false;
          }
          request.Headers.Add("Connection", "Keep-Alive");
          // Update your resource name
          request.Headers.Add("User-Agent", "translatorResourceGroup");
          // Audio output format. See API reference for full list.
          request.Headers.Add("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");
          // Create a request
          using (HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false))
          {
            response.EnsureSuccessStatusCode();
            // Asynchronously read the response
            using (Stream dataStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
              using (FileStream fileStream = new FileStream(@"Output.wav", FileMode.Create, FileAccess.Write, FileShare.Write))
              {
                  await dataStream.CopyToAsync(fileStream).ConfigureAwait(false);
                  fileStream.Close();
              }
            }
          }
        }
      }
      // Console.WriteLine("Audio file creation was successful");
      return true;
    }
  }
}