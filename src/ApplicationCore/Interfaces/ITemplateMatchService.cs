namespace DD2Bot.ApplicationCore.Interfaces
{
    public interface ITemplateMatchService
    {
        Task<TemplateMatch?> Match(string source, Template template, float threshold);
    }
}
