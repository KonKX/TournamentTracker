﻿using System;
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
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connections.GetTeamAll();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();
        public CreateTournamentForm()
        {
            InitializeComponent();
            WireUpLists();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {

            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;
            if (t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);
                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            selectedPrizes.Add(model);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void removeSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            //PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;
            //if (p != null)
            //{
            //    selectedTeamMembers.Remove(p);
            //    availableTeamMembers.Add(p);
            //    WireUpLists();
            //}
            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem;
            if (t != null)
            {
                selectedTeams.Remove(t);
                availableTeams.Add(t);
                WireUpLists();
            }
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            //PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;
            //if (p != null)
            //{
            //    selectedTeamMembers.Remove(p);
            //    availableTeamMembers.Add(p);
            //    WireUpLists();
            //}
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;
            if (p != null)
            {
                selectedPrizes.Remove(p);
                //Not deleting prizes...
                WireUpLists();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // Create tournament entry
            // Create prize entries
            // Create team entries
            // ...
            // Create matchups

            //Validate data
            decimal fee = 0;
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out fee);

            if (!feeAcceptable)
            {
                MessageBox.Show("Please enter a valid entry fee",
                    "Invalid Fee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            //Create tournament model
            TournamentModel tm = new TournamentModel();

            tm.TournamentName = tournamentNameValue.Text;
            tm.EntryFee = fee;
            tm.Prizes = selectedPrizes;
            tm.EnteredTeams = selectedTeams;
            
            // Wire the matchups
            TournamentLogic.CreateRounds(tm);

            GlobalConfig.Connections.CreateTournament(tm);
        }
    }
}
