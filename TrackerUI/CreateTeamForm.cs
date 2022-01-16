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
    public partial class CreateTeamForm : Form
    {
        private ITeamRequester callingFrom;

        private List<PersonModel> availableTeamMembers = GlobalConfig.Connections.GetPersonAll();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();

            //AddSampleData();
            callingFrom = caller;

            WireUpLists();
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {

                PersonModel model = new PersonModel(
                    firstNameValue.Text,
                    lastNameValue.Text,
                    emailValue.Text);

                model = GlobalConfig.Connections.CreatePerson(model);
                selectedTeamMembers.Add(model);
                WireUpLists();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
            }
            else
            {
                MessageBox.Show("The form has invalid information!");
            }
        }

        private void addTeamMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);
                WireUpLists();

            }
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;
            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);
                WireUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel model = new TeamModel();

            model.TeamName = teamNameValue.Text;
            model.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connections.CreateTeam(model);

            callingFrom.TeamComplete(model);
            this.Close();
        }

        private bool ValidateForm()
        {
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }
            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }
            if (emailValue.Text.Length == 0)
            {
                return false;
            }

            return true;
        }

        private void AddSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Silky", LastName = "Butt" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Test", LastName = "123" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Agent", LastName = "Smith" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Thomas", LastName = "Anderson" });
        }

        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;

            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;

            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }
    }
}
