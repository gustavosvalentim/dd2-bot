namespace DD2Bot.UI.Services
{
    public class PhaseCatalogService
    {
        private readonly TemplateMatchService _templateMatchService;

        #region Debug
        public string FoundPhaseName;
        public int FoundX;
        public int FoundY;
        public float FoundMatchPercentage;
        #endregion

        public PhaseCatalogService()
        {
            _templateMatchService = new TemplateMatchService();
        }

        private void PhaseFound(string phaseName, int x, int y, float matchPercentage)
        {
            FoundPhaseName = phaseName;
            FoundX = x;
            FoundY = y;
            FoundMatchPercentage = matchPercentage;
        }

        public bool IsBuildPhase(string screenImagePath)
        {
            var templateName = "vs-debug-actions";
            var threshold = 0.8f;
            return _templateMatchService.MatchTemplate(screenImagePath, templateName, threshold);
        }
    }
}
