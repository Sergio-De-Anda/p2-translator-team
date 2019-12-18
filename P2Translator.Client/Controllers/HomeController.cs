using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly HttpClientModel _http = new HttpClientModel();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> MessageBoard()
        {
          string url = "http://localhost:5050/api/translator/getmessages";
            HttpClient request = new HttpClient();
            var response = await request.GetAsync(url);
            List<MessageViewModel> allMessages = JsonConvert.DeserializeObject<List<MessageViewModel>>(response.Content.ReadAsStringAsync().Result);
            ViewBag.Messages = allMessages;
            // ViewBag.UserLanguage = "English";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MessageBoard(MessageBoardViewModel board)
        {
          string url = $"http://localhost:5050/api/translator/getmessages/{board.Language}";
            HttpClient request = new HttpClient();
            var response = await request.GetAsync(url);
            List<MessageViewModel> allMessages = JsonConvert.DeserializeObject<List<MessageViewModel>>(response.Content.ReadAsStringAsync().Result);
            ViewBag.Messages = allMessages;
            // ViewBag.UserLanguage = "English";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageViewModel m)
        {
          if(ModelState.IsValid)
          {
            
            string url = $"http://localhost:5050/api/translator/post";
            HttpClient request = new HttpClient();
            var response = await request.PostAsJsonAsync(url, m);
            return RedirectToAction("MessageBoard", "Home");
          }
          return RedirectToAction("MessageBoard", "Home");
          
        }
        public async Task<IActionResult> Index()
        { 
            string url = "http://localhost:5050/api/translator/getmessages";
            HttpClient request = new HttpClient();
            var response = await request.GetAsync(url);
            // response.Content
            // var response = client(request).Result;
            // var jsonResponse = response.Content.ReadAsStringAsync().Result;
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
    }
}
