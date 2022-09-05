using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace DD2Bot.UI.Services
{
    public class TemplateMatchService
    {
        private bool FindMatchPosition(Image<Gray, float> matchImage, float threshold)
        {
            for (int y = 0; y < matchImage.Data.GetLength(0); y++)
            {
                for (int x = 0; x < matchImage.Data.GetLength(1); x++)
                {
                    var matchPercentage = matchImage.Data[y, x, 0];
                    if (matchPercentage >= threshold)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FindMatchPosition(Image<Gray, float> matchImage, float threshold, List<Tuple<int, int, float>> posRecords)
        {
            for (var i = 0; i < posRecords.Count; i++)
            {
                var record = posRecords[i];
                var matchPercentage = matchImage.Data[record.Item1, record.Item2, 0];
                if (matchPercentage >= threshold)
                {
                    return true;
                }
            }
            return false;
        }

        public bool MatchTemplate(string sourceImagePath, string templateName, float threshold)
        {
            var sourceImage = new Image<Bgr, byte>(sourceImagePath);
            var templateImage = new Image<Bgr, byte>(Configuration.TemplatesPath + $"\\{templateName}.png");
            var matchImage = sourceImage.MatchTemplate(templateImage, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
            var positionRecordsPath = Configuration.RecordsPath + $"\\{templateName}_pos-records.json";
            var positionRecordsInfo = new FileInfo(positionRecordsPath);
            if (positionRecordsInfo.Exists)
            {
                var positionRecordsMatch = false;
                using (var positionRecordsStream = positionRecordsInfo.OpenRead())
                {
                    var posRecordsObject = (List<Tuple<int, int, float>>)new DataContractJsonSerializer(typeof(List<Tuple<int, int, float>>))
                        .ReadObject(positionRecordsStream);
                    positionRecordsMatch = FindMatchPosition(matchImage, threshold, posRecordsObject);
                }
                return positionRecordsMatch;
            }
            return FindMatchPosition(matchImage, threshold);
        }
    }
}
