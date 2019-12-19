namespace P2Translator.WebApi.Models
{
  public class Voice
  {
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Gender { get; set; }
    public string Locale { get; set; }
    public string SampleRateHertz { get; set; }
    public string VoiceType { get; set; }
  }
}