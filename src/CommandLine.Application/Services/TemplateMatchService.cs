using Core;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace CommandLine.Application.Services
{
    public class TemplateMatchService : ITemplateMatchService
    {
        private readonly IImageRepository _imageRepository;

        public TemplateMatchService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        private object[] MatchTemplate(Mat source, Mat template, float threshold)
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

        public async Task<TemplateMatch?> MatchSource(string source, string templateName, float threshold)
        {
            var sourceImageBuffer = await _imageRepository.GetImageAsync(source);
            return await MatchSource(sourceImageBuffer, templateName, threshold);
        }

        public async Task<TemplateMatch?> MatchSource(byte[] source, string templateName, float threshold)
        {
            Mat sourceImage = new Mat();
            Mat templateImage = new Mat();
            var templateImageBuffer = await _imageRepository.GetImageAsync(templateName);
            CvInvoke.Imdecode(source, ImreadModes.AnyColor, sourceImage);
            CvInvoke.Imdecode(templateImageBuffer, ImreadModes.AnyColor, templateImage);
            var matchResult = MatchTemplate(sourceImage, templateImage, threshold);
            if (matchResult.Length == 0)
            {
                return null;
            }
            return new TemplateMatch(templateName, (int)matchResult[0], (int)matchResult[1], threshold, (float)matchResult[2]);
        }
    }
}
