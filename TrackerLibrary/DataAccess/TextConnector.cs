using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.TextConnection;

namespace TrackerLibrary
{
    public class TextConnector : IDataConnection
    {
        //TODO - Wire up the CreatePrize for text files
        public PrizeModel CreatePrize(PrizeModel model)
        {
            model.Id = 1;

            return model;

            //Load the text file
            //Convert the text to List<PrizeModel>
            //Find the ID
            //Add the new record with the new ID
            //Convert the prizes to List<string>
            //Save the list<string> to the text file
        }
    }
}
