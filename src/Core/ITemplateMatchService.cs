namespace Core
{
    public interface ITemplateMatchService
    {
        Task<TemplateMatch?> MatchSource(string source, string templateName, float threshold);
    }
}
