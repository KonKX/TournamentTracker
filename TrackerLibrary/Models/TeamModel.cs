using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
       
    }
}
