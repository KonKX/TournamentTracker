﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        // Order randomly the list of teams
        // Check if the number of teams is big enough - if not add in byes 2*2*2*2 - 2^4
        // Create the first round of matchups
        // Create every round after that - 8 matchups - 4 matchups - 2 matchups - 1 matchup

        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = FindNumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(randomizedTeams, byes));
            CreateOtherRounds(model, rounds);
        }
        public static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();

            while (round <= rounds)
            {
                foreach (MatchupModel match in previousRound)
                {
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });
                    if (currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }
                model.Rounds.Add(currRound);
                previousRound = currRound;

                currRound = new List<MatchupModel>();
                round++;
            }
        }
        private static List<MatchupModel> CreateFirstRound(List<TeamModel> teams, int byes)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team, TeamCompetingId = team.Id });
                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new MatchupModel();

                    if (byes > 0)
                    {
                        byes--;
                    }
                }
            }

            return output;
        }
        private static int FindNumberOfByes(int rounds, int numberOfTeams)
        {
            //Math.Pow(2, rounds) takes in double (not safe to convert)
            int totalTeams = 2;

            for(int i = 1; i < rounds; i++)
            {
                totalTeams *= 2;
            }

            return totalTeams - numberOfTeams;

        }
        private static int FindNumberOfRounds(int teamCount)
        {
            int val = 2;
            int output = 1;

            while (val < teamCount)
            {
                output++;
                val *= 2;
            }

            return output;
        }
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            // Guid is unique
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
