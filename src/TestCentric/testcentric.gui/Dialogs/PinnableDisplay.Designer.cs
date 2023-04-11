// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Gui.Dialogs
{
    partial class PinnableDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PinnableDisplay));
            this.testName = new System.Windows.Forms.Label();
            this.pinButton = new System.Windows.Forms.CheckBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // testName
            // 
            this.testName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testName.BackColor = System.Drawing.SystemColors.Window;
            this.testName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testName.Location = new System.Drawing.Point(8, 6);
            this.testName.Margin = new System.Windows.Forms.Padding(0);
            this.testName.Name = "testName";
            this.testName.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.testName.Size = new System.Drawing.Size(224, 18);
            this.testName.TabIndex = 0;
            // 
            // pinButton
            // 
            this.pinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pinButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.pinButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pinButton.ForeColor = System.Drawing.SystemColors.Window;
            this.pinButton.Image = ((System.Drawing.Image)(resources.GetObject("pinButton.Image")));
            this.pinButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pinButton.Location = new System.Drawing.Point(236, 4);
            this.pinButton.Name = "pinButton";
            this.pinButton.Size = new System.Drawing.Size(20, 20);
            this.pinButton.TabIndex = 3;
            this.pinButton.UseVisualStyleBackColor = true;
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.ForeColor = System.Drawing.SystemColors.Window;
            this.exitButton.Image = ((System.Drawing.Image)(resources.GetObject("exitButton.Image")));
            this.exitButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.exitButton.Location = new System.Drawing.Point(261, 4);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(20, 20);
            this.exitButton.TabIndex = 4;
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // PinnableDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.testName);
            this.Controls.Add(this.pinButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PinnableDisplay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.PinnableDisplay_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label testName;
        private System.Windows.Forms.CheckBox pinButton;
        private System.Windows.Forms.Button exitButton;

        #endregion

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
