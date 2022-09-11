namespace DD2Bot.ApplicationCore
{
    public class TemplateMatch
    {
        public Template Template { get; }
        public int XPosition { get; }
        public int YPosition { get; }
        public float Threshold { get; }
        public float MatchPercentage { get; }
        public DateTime CreatedAt { get; }

        public TemplateMatch(Template template, int xPosition, int yPosition, float threshold, float matchPercentage)
        {
            Template = template;
            XPosition = xPosition;
            YPosition = yPosition;
            Threshold = threshold;
            MatchPercentage = matchPercentage;
            CreatedAt = DateTime.Now;
        }
    }
}
