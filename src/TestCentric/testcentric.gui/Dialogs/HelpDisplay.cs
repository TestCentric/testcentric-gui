// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCentric.Gui.Dialogs
{
    public class HelpDisplay : TestCentricFormBase
    {
        public HelpDisplay(CommandLineOptions options)
        { 
            InitializeComponent();

            if (options.ErrorMessages.Count == 0)
                errorList.Hide();
            else
            {
                var NL = Environment.NewLine;
                var sb = new StringBuilder($"Error(s) in command line:{NL}");
                foreach (string msg in options.ErrorMessages)
                    sb.Append($"  {msg}{NL}");
                errorList.Text = sb.ToString();
                errorList.Show();

                usageHeading.Top = errorList.Bottom + 8;
                helpMessage.Top = usageHeading.Bottom + 8;
            }

            StringWriter writer = new StringWriter();

            writer.WriteLine("""
                Starts the TestCentric Runner, optionally loading and running a set of NUnit
                tests. You may specify any combination of assemblies and supported project 
                files as arguments.

                InputFiles:
                  One or more assemblies or test projects of a recognized type.");
                  If no input files are given, the tests contained in the most");
                  recently used project or assembly are loaded, unless the");
                  --noload option is specified");

                Options:
                """);

            options.WriteOptionDescriptions(writer);

            helpMessage.Text = writer.GetStringBuilder().ToString();
        }

        private System.Windows.Forms.Label usageHeading;
        private System.Windows.Forms.Label errorList;
        private System.Windows.Forms.Label helpMessage;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpDisplay));
            this.usageHeading = new System.Windows.Forms.Label();
            this.helpMessage = new System.Windows.Forms.Label();
            this.errorList = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // usageHeading
            // 
            this.usageHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usageHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usageHeading.Location = new System.Drawing.Point(12, 9);
            this.usageHeading.Name = "usageHeading";
            this.usageHeading.Size = new System.Drawing.Size(593, 21);
            this.usageHeading.TabIndex = 14;
            this.usageHeading.Text = "Usage: TESTCENTRIC [inputfiles] [options]";
            // 
            // helpMessage
            // 
            this.helpMessage.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpMessage.Location = new System.Drawing.Point(12, 42);
            this.helpMessage.Name = "helpMessage";
            this.helpMessage.Size = new System.Drawing.Size(583, 717);
            this.helpMessage.TabIndex = 15;
            this.helpMessage.Text = "helpMessage";
            // 
            // errorList
            // 
            this.errorList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorList.AutoSize = true;
            this.errorList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorList.ForeColor = System.Drawing.Color.Red;
            this.errorList.Location = new System.Drawing.Point(12, 9);
            this.errorList.Name = "errorList";
            this.errorList.Size = new System.Drawing.Size(70, 17);
            this.errorList.TabIndex = 16;
            this.errorList.Text = "errorList";
            // 
            // HelpDisplay
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(620, 779);
            this.Controls.Add(this.helpMessage);
            this.Controls.Add(this.usageHeading);
            this.Controls.Add(this.errorList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpDisplay";
            this.Text = "TestCentric Runner Command-line Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
