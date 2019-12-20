using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using P2Translator.Client.Models;



namespace P2Translator.Client.Controllers
{
  [Route("/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Indexx()
        {
            ViewData["Message"] = "Hello from webfrontend";

            // Call *api*, and display its response in the page
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://api/Translator/SendId/5");
            var response = await client.SendAsync(request);
            ViewData["Message"] += " and " + await response.Content.ReadAsStringAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MessageBoard()
        {
          var client = new HttpClient();
          var request = new HttpRequestMessage();
          request.RequestUri = new Uri("http://api/Translator/GetMessages");
          var response = await client.GetAsync(request.RequestUri);
          List<MessageViewModel> allMessages = JsonConvert.DeserializeObject<List<MessageViewModel>>(response.Content.ReadAsStringAsync().Result);
          ViewBag.Messages = allMessages;
          ViewBag.Languages = GetLanguages().Result;
          return View();
        }
        [HttpGet("{id}")]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Message(int id)
        {
          string url = $"http://api/Translator/getmessage/{id}";
          HttpClient request = new HttpClient();
          var response = await request.GetAsync(url);
          MessageViewModel message = JsonConvert.DeserializeObject<MessageViewModel>(response.Content.ReadAsStringAsync().Result);
          ViewBag.Message = message;
          await Task.FromResult(CreateAudioFile(message));
          return View("Message");
        }
        [HttpPost]
        public async Task<IActionResult> MessageBoard(MessageBoardViewModel board)
        {
          string url = $"http://api/Translator/getmessages/{board.Language}";
          HttpClient request = new HttpClient();
          var response = await request.GetAsync(url);
          List<MessageViewModel> allMessages = JsonConvert.DeserializeObject<List<MessageViewModel>>(response.Content.ReadAsStringAsync().Result);
          ViewBag.Messages = allMessages;
          ViewBag.Languages = GetLanguages().Result;
          return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageViewModel m)
        {
          if(ModelState.IsValid)
          {
            string url = $"http://api/Translator/post";
            HttpClient request = new HttpClient();
            await request.PostAsJsonAsync(url, m);
            return RedirectToAction("MessageBoard", "Home");
          }
          return RedirectToAction("MessageBoard", "Home");
          
        }
        public async Task<IActionResult> Index()
        { 
            string url = "http://api/Translator/getmessages";
            HttpClient request = new HttpClient();
            var response = await request.GetAsync(url);
            var deserialized = JsonConvert.DeserializeObject<List<MessageViewModel>>(response.Content.ReadAsStringAsync().Result);
            foreach(var m in deserialized)
              Console.WriteLine(m.Content);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<List<string>> GetLanguages()
        {
          var client = new HttpClient();
          var requestGetLanguages = new HttpRequestMessage();
          requestGetLanguages.RequestUri = new Uri("http://api/Translator/GetLanguages");
          var responseLanguages = await client.GetAsync(requestGetLanguages.RequestUri);
          List<string> allLanguges = JsonConvert.DeserializeObject<List<string>>(responseLanguages.Content.ReadAsStringAsync().Result);
         
          return allLanguges;
        }

        private async Task CreateAudioFile(MessageViewModel m)
        {
          string url = $"http://api/Translator/CreateAudio";
          HttpClient request = new HttpClient();
          await request.PostAsJsonAsync(url, m);
        }
    }
}
