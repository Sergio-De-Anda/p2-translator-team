using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace P2Translator.Client.Models 
{
  public class HttpClientModel
  {
    private HttpClient client = new HttpClient();
    public void RunAsync()
    {
      // Update port # in the following line.
      client.BaseAddress = new Uri("http://localhost:5000/");
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
      new MediaTypeWithQualityHeaderValue("application/json"));
    }
  }
}