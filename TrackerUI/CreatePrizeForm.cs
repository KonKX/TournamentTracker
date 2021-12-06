using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        public CreatePrizeForm()
        {
            InitializeComponent();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PrizeModel model = new PrizeModel(
                    placeNameValue.Text, 
                    placeNumberValue.Text, 
                    prizeAmountValue.Text, 
                    prizePercentageValue.Text);

               
                    GlobalConfig.Connections.CreatePrize(model);
                

                placeNameValue.Text="";
                placeNumberValue.Text="0";
                prizeAmountValue.Text="0";
                prizePercentageValue.Text="0";
            }
            else 
            {
                MessageBox.Show("The form has invalid information!");
            }
        }

        private void placeNumberLabel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void orLabel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private bool ValidateForm()
        {

            if (!int.TryParse(placeNumberValue.Text, out int placeNum) 
                || placeNum < 1 
                || placeNameValue.Text.Length == 0 
                || !decimal.TryParse(prizeAmountValue.Text, out decimal prizeAmountValid) 
                || !int.TryParse(prizePercentageValue.Text, out int prizePercentageValid)
                || (prizeAmountValid<=0 && prizePercentageValid<=0)
                || prizePercentageValid < 0
                || prizePercentageValid > 100)
            {
                return false;
            }

            return true;
        }
    }
}
