using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P2Translator.Data;
using P2Translator.Data.Models;
using P2Translator.WebApi.Models;
using System;
using Newtonsoft.Json;

namespace P2Translator.WebApi.Controllers 
{
  [Produces("application/json")]
  [Route("[controller]/[action]")]
  [ApiController]
  public class TranslatorController : ControllerBase
  {
    
    private readonly P2TranslatorDbContext _db;
    public TranslatorController(P2TranslatorDbContext _db)
    {
        this._db = _db;
    }
    [HttpGet]
    public async Task<IActionResult> GetMessages()
    {
      return await Task.FromResult(Ok(_db.Message.ToList()));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessage(int id)
    {
      var result = _db.Message.SingleOrDefault(m => m.MessageId == id);
      if(result == null)
        {
        return await Task.FromResult(NotFound());
        }
        return await Task.FromResult(Ok(result));
    }

    [HttpGet("{language}")]
    public async Task<IActionResult> GetMessages(string language)
    {
      List<Message> messages = _db.Message.ToList();
      List<Message> translatedMessageList = new List<Message>();
      Translator tr = new Translator();
      foreach(Message m in messages)
      {
        Message translatedMessage = new Message();
        translatedMessage.Content = await tr.Translate(m.Content, language);
        translatedMessage.MessageDateTime = m.MessageDateTime;
        translatedMessage.MessageId = m.MessageId;
        translatedMessageList.Add(translatedMessage);
      }
      return await Task.FromResult(Ok(translatedMessageList));
    }
    [HttpPost]
    public async Task<IActionResult> Post(Message m)
    {
      if(ModelState.IsValid)
      {
        m.MessageDateTime = DateTime.Now;
        _db.Message.Add(m);
        await _db.SaveChangesAsync();
        return await Task.FromResult(Ok(m));
      }
      return await Task.FromResult(NotFound(m));
    }
    [HttpGet]
    public async Task<IActionResult> GetLanguages()
    {
      Translator languages = new Translator();
      return await Task.FromResult(Ok(languages.GetLanguages()));
    }
    [HttpGet]
    public async Task<IActionResult> GetVoices()
    {
      Speech sch = new Speech();
      return await Task.FromResult(Ok(sch.GetVoices().Result));
    }
    [HttpPost]
    public async Task<IActionResult> CreateAudio(Message m)
    {
      Speech sch = new Speech();
      // testing
      if(await sch.TextToSpeech("Hello World", "English"))
        return await Task.FromResult(Ok(m));
      return await Task.FromResult(BadRequest(m));
    }
  }
}