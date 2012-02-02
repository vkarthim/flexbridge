﻿namespace FwdataTestApp
{
	partial class NestFwdataFile
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NestFwdataFile));
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this._btnBrowse = new System.Windows.Forms.Button();
			this._fwdataPathname = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this._btnNest = new System.Windows.Forms.Button();
			this._cbNestFile = new System.Windows.Forms.CheckBox();
			this._cbRoundTripData = new System.Windows.Forms.CheckBox();
			this._cbVerify = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			//
			// _openFileDialog
			//
			this._openFileDialog.Filter = "Fwdata Files|*.fwdata";
			//
			// _btnBrowse
			//
			this._btnBrowse.Location = new System.Drawing.Point(479, 11);
			this._btnBrowse.Name = "_btnBrowse";
			this._btnBrowse.Size = new System.Drawing.Size(66, 23);
			this._btnBrowse.TabIndex = 11;
			this._btnBrowse.Text = "Browse...";
			this._btnBrowse.UseVisualStyleBackColor = true;
			this._btnBrowse.Click += new System.EventHandler(this.BrowseForFile);
			//
			// _fwdataPathname
			//
			this._fwdataPathname.Enabled = false;
			this._fwdataPathname.Location = new System.Drawing.Point(70, 12);
			this._fwdataPathname.Name = "_fwdataPathname";
			this._fwdataPathname.Size = new System.Drawing.Size(403, 20);
			this._fwdataPathname.TabIndex = 10;
			this._fwdataPathname.WordWrap = false;
			//
			// label1
			//
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "fwdata file:";
			//
			// _btnNest
			//
			this._btnNest.Enabled = false;
			this._btnNest.Location = new System.Drawing.Point(7, 94);
			this._btnNest.Name = "_btnNest";
			this._btnNest.Size = new System.Drawing.Size(107, 23);
			this._btnNest.TabIndex = 8;
			this._btnNest.Text = "Run Selected";
			this._btnNest.UseVisualStyleBackColor = true;
			this._btnNest.Click += new System.EventHandler(this.RunSelected);
			//
			// _cbNestFile
			//
			this._cbNestFile.AutoSize = true;
			this._cbNestFile.Location = new System.Drawing.Point(7, 42);
			this._cbNestFile.Name = "_cbNestFile";
			this._cbNestFile.Size = new System.Drawing.Size(67, 17);
			this._cbNestFile.TabIndex = 12;
			this._cbNestFile.Text = "Nest File";
			this._cbNestFile.UseVisualStyleBackColor = true;
			//
			// _cbRoundTripData
			//
			this._cbRoundTripData.AutoSize = true;
			this._cbRoundTripData.Checked = true;
			this._cbRoundTripData.CheckState = System.Windows.Forms.CheckState.Checked;
			this._cbRoundTripData.Location = new System.Drawing.Point(7, 66);
			this._cbRoundTripData.Name = "_cbRoundTripData";
			this._cbRoundTripData.Size = new System.Drawing.Size(105, 17);
			this._cbRoundTripData.TabIndex = 13;
			this._cbRoundTripData.Text = "Round Trip Data";
			this._cbRoundTripData.UseVisualStyleBackColor = true;
			//
			// _cbVerify
			//
			this._cbVerify.AutoSize = true;
			this._cbVerify.Checked = true;
			this._cbVerify.CheckState = System.Windows.Forms.CheckState.Checked;
			this._cbVerify.Location = new System.Drawing.Point(116, 66);
			this._cbVerify.Name = "_cbVerify";
			this._cbVerify.Size = new System.Drawing.Size(102, 17);
			this._cbVerify.TabIndex = 14;
			this._cbVerify.Text = "Verify It Worked";
			this._cbVerify.UseVisualStyleBackColor = true;
			//
			// NestFwdataFile
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.ClientSize = new System.Drawing.Size(554, 123);
			this.Controls.Add(this._cbVerify);
			this.Controls.Add(this._cbRoundTripData);
			this.Controls.Add(this._cbNestFile);
			this.Controls.Add(this._btnBrowse);
			this.Controls.Add(this._fwdataPathname);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._btnNest);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NestFwdataFile";
			this.Text = "Test an fwdata file";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog _openFileDialog;
		private System.Windows.Forms.Button _btnBrowse;
		private System.Windows.Forms.TextBox _fwdataPathname;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button _btnNest;
		private System.Windows.Forms.CheckBox _cbNestFile;
		private System.Windows.Forms.CheckBox _cbRoundTripData;
		private System.Windows.Forms.CheckBox _cbVerify;
	}
}