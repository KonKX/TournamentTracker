using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TrackerLibrary.TextConnection
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ConfigurationManager.AppSettings["filepath"]} \\ {fileName}";
            
        }
        
        public static List<String> LoadFile(this string fileName)
        {
            if (!File.Exists(fileName))
            {
                return new List<string>();
            }

            return File.ReadAllLines(fileName).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<String> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();
            foreach (String line in lines)
            {
                string[] parts = line.Split(',');
                output.Add(new PrizeModel
                {
                    Id = int.Parse(parts[0]),
                    PlaceNumber = int.Parse(parts[1]),
                    PlaceName = parts[2],
                    PrizeAmount = decimal.Parse(parts[3]),
                    PrizePercentage = double.Parse(parts[4])
                });
            }
            return output;
        }
    }
}
