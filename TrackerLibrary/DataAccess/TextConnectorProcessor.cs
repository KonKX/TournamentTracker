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
        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines, string teamFileName, string peopleFileName, string prizesFileName)
        {
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizes = prizesFileName.FullFilePath().LoadFile().ConvertToPrizeModels();
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();


            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(parts[0]);
                tm.TournamentName = parts[1];
                tm.EntryFee = decimal.Parse(parts[2]);

                string[] teamIds = parts[3].Split('|');

                foreach(string teamId in teamIds)
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(teamId)).First());
                }

                string[] prizeIds = parts[4].Split('|');

                foreach (string prizeId in prizeIds)
                {
                    tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(prizeId)).First());
                }

                // TODO: Capture rounds info
                
                string[] rounds = parts[5].Split('|');
                
                foreach (string round in rounds)
                {
                    string[] msText = round.Split('^');
                    List<MatchupModel> ms = new List<MatchupModel>();

                    foreach (string matchupModelTextId in msText)
                    {
                        ms.Add(matchups.Where( x => x.Id == int.Parse(matchupModelTextId)).First());
                    }

                    tm.Rounds.Add(ms);
                }

                output.Add(tm);

            }
            return output;
        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName},{ p.Email }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        
        public static void SaveToTournamentFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (TournamentModel t in models)
            {
                lines.Add($"{ t.Id }," +
                    $"{ t.TournamentName }," +
                    $"{ t.EntryFee }," +
                    $"{ ConvertTeamListToString(t.EnteredTeams) }," +
                    $"{ ConvertPrizeListToString(t.Prizes) }," +
                    $"{ ConvertRoundListToString(t.Rounds) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        public static void SaveRoundsToFile(this TournamentModel model, string matchupFile, string matchupEntryFile)
        {
            // Loop through each round
            // Loop through each matchup
            // Get the id for the new matchup and save the record
            // Loop through each entry, get the id and save it

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    // Load all of the matchups from file
                    // Get the top id and add one
                    // Store the id
                    // Save the matchup record
                    matchup.SaveMatchupToFile(matchupFile, matchupEntryFile);
                }
            }
        }

        public static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
        {
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                
                int parentId = 0;
                MatchupModel parentMatchup;

                if (int.TryParse(parts[3], out parentId))
                {
                    parentMatchup = LookupMatchupById(parentId);
                }
                else
                {
                    parentMatchup = null;
                }

                TeamModel teamCompeting;

                if (parts[1].Length == 0 )
                {
                    teamCompeting = null;
                }
                else
                {
                    teamCompeting = LookupTeamById(int.Parse(parts[1]));
                }
                output.Add(new MatchupEntryModel
                {
                    Id = int.Parse(parts[0]),
                    TeamCompeting = teamCompeting,
                    Score = double.Parse(parts[2]),
                    ParentMatchup = parentMatchup
                });
            }
            return output;
        }

        private static List<MatchupEntryModel> ConvertStringToMatchupEntryModels(string input)
        {
            string[] ids = input.Split('|');
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();
           
            foreach (string id in ids)
            {
               output.Add(entries.Where(x => x.Id == int.Parse(id)).First());
            }

            return output;
        }
        private static TeamModel LookupTeamById(int id)
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(GlobalConfig.PeopleFile);
          
            return teams.Where(x => x.Id == id).First();
        }
        private static MatchupModel LookupMatchupById(int id)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();

            return matchups.Where(x => x.Id == id).First();
        }
        public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                TeamModel winner;
                if (parts[2].Length == 0)
                {
                    winner = null;
                }
                else
                {
                    winner = LookupTeamById(int.Parse(parts[2]));
                }

                output.Add(new MatchupModel
                {
                    Id = int.Parse(parts[0]),
                    Entries = ConvertStringToMatchupEntryModels(parts[1]),
                    Winner = winner,
                    MatchupRound = int.Parse(parts[3])
                });
            }
            return output;
        }
        public static void SaveMatchupToFile(this MatchupModel matchup, string matchupFile, string matchupEntryFile)
        {

            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();

            int currentId = 1;

            if (matchups.Count > 0)
            {
                currentId = matchups.OrderByDescending(x => x.Id).First().Id + 1;
            }

            matchup.Id = currentId;

            matchups.Add(matchup);

            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile(matchupEntryFile);
            }

            //save to file
            var lines = new List<string>();
            foreach (MatchupModel m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }
                lines.Add($"{ m.Id },{ ConvertMatchupEntryListToString(m.Entries) },{ winner },{ m.MatchupRound}");
            }
            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        public static void SaveEntryToFile(this MatchupEntryModel entry, string matchupEntryFile)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();

            int currentId = 1;

            if (entries.Count > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }

            entry.Id = currentId;
            entries.Add(entry);

            //save to file
            List<string> lines = new List<string>();
            foreach (MatchupEntryModel e in entries)
            {
                string parent = "";
                if (e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }

                string teamCompeting = "";
                if (e.TeamCompeting != null)
                {
                    teamCompeting = e.TeamCompeting.Id.ToString();
                }


                lines.Add($"{ e.Id },{ teamCompeting },{ e.Score },{ parent }");
            }

            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
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
        public static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel t in teams)
            {
                output += $"{ t.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
        public static string ConvertMatchupEntryListToString(List<MatchupEntryModel> entries)
        {
            string output = "";

            if (entries.Count == 0)
            {
                return "";
            }

            foreach (MatchupEntryModel e in entries)
            {
                output += $"{ e.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
        public static string ConvertPrizeListToString(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel p in prizes)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
        public static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
        public static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel m in matchups)
            {
                output += $"{ m.Id }^";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

    }
}
