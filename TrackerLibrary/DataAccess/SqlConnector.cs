using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//@PlaceNumber int,
//@PlaceName nvarchar(50),
//@PrizeAmount money,
//@PrizePercentage float,
//@id int = 0 output

namespace TrackerLibrary
{
    public class SqlConnector : IDataConnection
    {
        /// <summary>
        /// Saves a new person to the database
        /// </summary>
        /// <param name="model">The person information</param>
        /// <returns>The person information, including the unique identifier</returns>
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@Email", model.Email);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");
                return model;
            }
        }

        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize information, including the unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id",0, dbType : DbType.Int32, direction: ParameterDirection.Output);

                con.Execute("dbo.spPrizes_Insert", p , commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                foreach (PersonModel tm in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", tm.Id);

                    con.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }

                return model;
            }
        }

        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                var p = new DynamicParameters();
                p.Add("@name", model.TournamentName);
                p.Add("@fee", model.EntryFee);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                foreach (PrizeModel pz in model.Prizes)
                {
                    p = new DynamicParameters();
                    p.Add("@tournamentid", model.Id);
                    p.Add("@prizeid", pz.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
                }

                foreach (TeamModel tm in model.EnteredTeams)
                {
                    p = new DynamicParameters();
                    p.Add("@tournamentid", model.Id);
                    p.Add("@teamid",tm.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
                }

                // Save tournament rounds
                foreach (List<MatchupModel> round in model.Rounds)
                {
                    foreach (MatchupModel m in round)
                    {
                        p = new DynamicParameters();
                        p.Add("@tournamentid", model.Id);
                        p.Add("@matchupround", m.MatchupRound);
                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        con.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);

                        m.Id = p.Get<int>("@id");

                        foreach (MatchupEntryModel entry in m.Entries)
                        {
                            p = new DynamicParameters();
                            p.Add("@matchupid", m.Id);
                            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                            con.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);
                        }
                    }
                }
            }
        }

        public List<PersonModel> GetPersonAll()
        {
            List<PersonModel> output = new List<PersonModel>();

            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                output = con.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }

            return output;
        }

        public List<TeamModel> GetTeamAll()
        {
            List<TeamModel> output = new List<TeamModel>();

            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                output = con.Query<TeamModel>("dbo.spTeam_GetAll").ToList();

                foreach (TeamModel tm in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", tm.Id);
                    tm.TeamMembers = con.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }

            return output;
        }
    }
}
