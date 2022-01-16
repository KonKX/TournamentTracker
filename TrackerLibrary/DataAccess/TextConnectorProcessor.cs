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
            return $"{ConfigurationManager.AppSettings["filepath"]}\\{fileName}";

        }

        public static List<string> LoadFile(this string fileName)
        {
            if (!File.Exists(fileName))
            {
                return new List<string>();
            }

            return File.ReadAllLines(fileName).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();
            foreach (string line in lines)
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

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                
                string[] parts = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(parts[0]);
                t.TeamName = parts[1];

                string[] personIds = parts[2].Split('|');
                foreach (string personId in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(personId)).First());
                }

                output.Add(t);
            }

            return output;
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                output.Add(new PersonModel
                {
                    Id = int.Parse(parts[0]),
                    FirstName = parts[1],
                    LastName = parts[2],
                    Email = parts[3]
                });
            }
            return output;
        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName},{ p.Email}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id }, { t.TeamName }, {ConvertPeopleListToString(t.TeamMembers)}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count == 0)
            {
                return "";
            }

            foreach (PersonModel p in people)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
    }
}
