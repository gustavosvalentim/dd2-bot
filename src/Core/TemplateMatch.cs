namespace Core
{
    public class TemplateMatch
    {
        public string TemplateName { get; }
        public int XPosition { get; }
        public int YPosition { get; }
        public float Threshold { get; }
        public float MatchPercentage { get; }
        public DateTime CreatedAt { get; }

        public TemplateMatch(string templateName, int xPosition, int yPosition, float threshold, float matchPercentage)
        {
            TemplateName = templateName;
            XPosition = xPosition;
            YPosition = yPosition;
            MatchPercentage = matchPercentage;
            CreatedAt = DateTime.Now;
        }
    }
}
