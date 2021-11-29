﻿namespace TrackerUI
{
    partial class TournamentDashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.createNewTournamentButton = new System.Windows.Forms.Button();
            this.loadExistingTournamentButton = new System.Windows.Forms.Button();
            this.loadExistingTournamentDropDown = new System.Windows.Forms.ComboBox();
            this.selectExistingTournamentLabel = new System.Windows.Forms.Label();
            this.headerLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // createNewTournamentButton
            // 
            this.createNewTournamentButton.BackColor = System.Drawing.Color.LightGray;
            this.createNewTournamentButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.createNewTournamentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.createNewTournamentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.createNewTournamentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createNewTournamentButton.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createNewTournamentButton.Location = new System.Drawing.Point(407, 308);
            this.createNewTournamentButton.Name = "createNewTournamentButton";
            this.createNewTournamentButton.Size = new System.Drawing.Size(156, 38);
            this.createNewTournamentButton.TabIndex = 39;
            this.createNewTournamentButton.Text = "Create New";
            this.createNewTournamentButton.UseVisualStyleBackColor = false;
            // 
            // loadExistingTournamentButton
            // 
            this.loadExistingTournamentButton.BackColor = System.Drawing.Color.LightGray;
            this.loadExistingTournamentButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.loadExistingTournamentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.loadExistingTournamentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.loadExistingTournamentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadExistingTournamentButton.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadExistingTournamentButton.Location = new System.Drawing.Point(244, 308);
            this.loadExistingTournamentButton.Name = "loadExistingTournamentButton";
            this.loadExistingTournamentButton.Size = new System.Drawing.Size(156, 38);
            this.loadExistingTournamentButton.TabIndex = 38;
            this.loadExistingTournamentButton.Text = "Load";
            this.loadExistingTournamentButton.UseVisualStyleBackColor = false;
            // 
            // loadExistingTournamentDropDown
            // 
            this.loadExistingTournamentDropDown.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.loadExistingTournamentDropDown.FormattingEnabled = true;
            this.loadExistingTournamentDropDown.Location = new System.Drawing.Point(244, 257);
            this.loadExistingTournamentDropDown.Name = "loadExistingTournamentDropDown";
            this.loadExistingTournamentDropDown.Size = new System.Drawing.Size(313, 38);
            this.loadExistingTournamentDropDown.TabIndex = 37;
            // 
            // selectExistingTournamentLabel
            // 
            this.selectExistingTournamentLabel.AutoSize = true;
            this.selectExistingTournamentLabel.BackColor = System.Drawing.Color.Transparent;
            this.selectExistingTournamentLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectExistingTournamentLabel.Location = new System.Drawing.Point(280, 204);
            this.selectExistingTournamentLabel.Name = "selectExistingTournamentLabel";
            this.selectExistingTournamentLabel.Size = new System.Drawing.Size(241, 37);
            this.selectExistingTournamentLabel.TabIndex = 36;
            this.selectExistingTournamentLabel.Text = "Select Tournament:";
            // 
            // headerLabel
            // 
            this.headerLabel.AutoSize = true;
            this.headerLabel.BackColor = System.Drawing.Color.Transparent;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerLabel.Location = new System.Drawing.Point(237, 105);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(326, 40);
            this.headerLabel.TabIndex = 35;
            this.headerLabel.Text = "Tournament Dashboard";
            // 
            // TournamentDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.createNewTournamentButton);
            this.Controls.Add(this.loadExistingTournamentButton);
            this.Controls.Add(this.loadExistingTournamentDropDown);
            this.Controls.Add(this.selectExistingTournamentLabel);
            this.Controls.Add(this.headerLabel);
            this.Name = "TournamentDashboardForm";
            this.Text = "Tournament Dashboard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createNewTournamentButton;
        private System.Windows.Forms.Button loadExistingTournamentButton;
        private System.Windows.Forms.ComboBox loadExistingTournamentDropDown;
        private System.Windows.Forms.Label selectExistingTournamentLabel;
        private System.Windows.Forms.Label headerLabel;
    }
}