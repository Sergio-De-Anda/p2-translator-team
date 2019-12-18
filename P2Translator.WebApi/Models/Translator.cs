using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace P2Translator.WebApi.Models
{
  public class Translator
  {
    private const string key_var = "TRANSLATOR_TEXT_SUBSCRIPTION_KEY";
    private static readonly string subscriptionKey = "49331ae9c7b548cdb0f97fb95b882d80";
    private const string endpoint_var = "TRANSLATOR_TEXT_ENDPOINT";
    private static readonly string endpoint = "https://api-nam.cognitive.microsofttranslator.com/";


    public Translator()
    {
      if (null == subscriptionKey)
      {
          throw new Exception("Please set/export the environment variable: " + key_var);
      }
      if (null == endpoint)
      {
          throw new Exception("Please set/export the environment variable: " + endpoint_var);
      }
    }
    // Async call to the Translator Text API
    static private async Task<string> TranslateTextRequest(string subscriptionKey, string endpoint, string route, string inputText)
    {
      object[] body = new object[] { new { Text = inputText } };
      var requestBody = JsonConvert.SerializeObject(body);

      using (var client = new HttpClient())
      using (var request = new HttpRequestMessage())
      {
          // Build the request.
          request.Method = HttpMethod.Post;
          request.RequestUri = new Uri(endpoint + route);
          request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
          request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

          // Send the request and get response.
          HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
          string result = await response.Content.ReadAsStringAsync();
          var deserializedOutput = JsonConvert.DeserializeObject<object>(result);
          JArray jArray = deserializedOutput as JArray;
          JValue translatedText = jArray.First.Last.First.First.First.First as JValue;
          return translatedText.Value.ToString();
      }
    }
    private static string GetLanguageCode(string lang)
    {
      var langMap = new Dictionary<string, string>();
      string route = "/languages?api-version=3.0";

      using (var client = new HttpClient())
      using (var request = new HttpRequestMessage())
      {
          // Set the method to GET
          request.Method = HttpMethod.Get;
          // Construct the full URI
          request.RequestUri = new Uri(endpoint + route);
          // Send request, get response
          var response = client.SendAsync(request).Result;
          var jsonResponse = response.Content.ReadAsStringAsync().Result;
          var deserialized = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
          var translation = deserialized.translation;
          // create map <language name, language code>
          foreach(var langCode in translation)
          {
            foreach(var langName in langCode)
            {
              langMap.Add(langName.name.ToString().ToLower(), langCode.Name.ToString().ToLower());
            }
          }
      }
      if (langMap.ContainsKey(lang))
      {
        return langMap[lang];
      }
      return null;
    }
    public List<string> GetLanguages()
    {
      var languages = new List<string>();
      string route = "/languages?api-version=3.0";
      using (var client = new HttpClient())
      using (var request = new HttpRequestMessage())
      {
        // Set the method to GET
        request.Method = HttpMethod.Get;
        // Construct the full URI
        request.RequestUri = new Uri(endpoint + route);
        // Send request, get response
        var response = client.SendAsync(request).Result;
        var jsonResponse = response.Content.ReadAsStringAsync().Result;
        var deserialized = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
        var translation = deserialized.translation;
        // create map <language name, language code>
        foreach(var langCode in translation)
        {
          foreach(var langName in langCode)
          {
            languages.Add(langName.name.ToString().ToLower());
          }
        }
      }
      return languages;
    }
    public async Task<string> Translate(string message, string ToLanguage)
    {
      string langCode = GetLanguageCode(ToLanguage.ToLower());
      if(string.IsNullOrEmpty(langCode) || string.IsNullOrEmpty(message))
        return null;
      string route = "/translate?api-version=3.0&to="+langCode;
      string translatedText = await Task.FromResult(TranslateTextRequest(subscriptionKey, endpoint, route, message.ToLower()).Result);
      return translatedText;
    }
  }
}