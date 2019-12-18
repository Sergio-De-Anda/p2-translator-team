using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P2Translator.Data;
using P2Translator.Data.Models;
using P2Translator.WebApi.Models;
using System;

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

    // GET Translator/SendId/5
    [HttpGet("{id}")]
    public async Task<ActionResult<string>> SendId(int id)
    {
        var TranslatorValue = "Translator (with value " + id + ")";
        return await Task.FromResult(Ok(TranslatorValue));
    }

    [HttpGet]
    public IActionResult GetLanguages()
    {
      List<string> languages = new List<string>();
      languages.Add("Spanish");
      languages.Add("English");
      return Ok(languages);
    }
    [HttpGet]
    public object GetMessage()
    {
      return _db.Message.Where(m => m.Content.Contains("Content")).Select((c) => new 
      {
        Id = c.MessageId,
        Content = c.Content,
        MessageDateTime = c.MessageDateTime
      }).ToList();
    }
    [HttpGet]
    public async Task<IActionResult> GetMessages()
    {
      return await Task.FromResult(Ok(_db.Message.ToList()));
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
  }
}