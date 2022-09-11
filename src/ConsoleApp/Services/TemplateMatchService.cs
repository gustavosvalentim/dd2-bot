using DD2Bot.ApplicationCore;
using DD2Bot.ApplicationCore.Interfaces;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace DD2Bot.ConsoleApp.Services
{
    public class TemplateMatchService : ITemplateMatchService
    {
        private readonly IFileRepository _imageRepository;

        public TemplateMatchService(IFileRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        private object[] MatchAndVerify(Mat source, Mat template, float threshold)
        {
            var matchImage = source.ToImage<Bgr, byte>()
                .MatchTemplate(template.ToImage<Bgr, byte>(), TemplateMatchingType.CcoeffNormed);
            for (int y = 0; y < matchImage.Data.GetLength(0); y++)
            {
                for (int x = 0; x < matchImage.Data.GetLength(1); x++)
                {
                    var matchPercentage = matchImage.Data[y, x, 0];
                    if (matchPercentage >= threshold)
                    {
                        return new object[3] { x, y, matchPercentage };
                    }
                }
            }
            return Array.Empty<object>();
        }

        public async Task<TemplateMatch?> Match(string source, Template template, float threshold)
        {
            var sourceImageBuffer = await _imageRepository.ReadAsync(source);
            return await Match(sourceImageBuffer, template, threshold);
        }

        public async Task<TemplateMatch?> Match(byte[] source, Template template, float threshold)
        {
            Mat sourceImage = new Mat();
            Mat templateImage = new Mat();
            var templateImageBuffer = await _imageRepository.ReadAsync(template.ImagePath);
            CvInvoke.Imdecode(source, ImreadModes.AnyColor, sourceImage);
            CvInvoke.Imdecode(templateImageBuffer, ImreadModes.AnyColor, templateImage);
            var matchResult = MatchAndVerify(sourceImage, templateImage, threshold);
            if (matchResult.Length == 0)
            {
                return null;
            }
            return new TemplateMatch(template, (int)matchResult[0], (int)matchResult[1], threshold, (float)matchResult[2]);
        }
    }
}
