namespace TestCentric.Gui.Views
{
    using System.Windows.Forms;

    partial class TestOutputSubView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.outputLabel = new System.Windows.Forms.Label();
            this.output = new TestCentric.Gui.Controls.ExpandingLabel();
            this.SuspendLayout();
            // 
            // outputLabel
            // 
            this.outputLabel.Location = new System.Drawing.Point(4, 2);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(66, 13);
            this.outputLabel.TabIndex = 0;
            this.outputLabel.Text = "Text Output:";
            // 
            // output
            // 
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.output.BackColor = System.Drawing.Color.LightYellow;
            this.output.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.output.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.Location = new System.Drawing.Point(10, 22);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(457, 40);
            this.output.TabIndex = 29;
            // 
            // TestOutputSubView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.output);
            this.Controls.Add(this.outputLabel);
            this.Name = "TestOutputSubView";
            this.Size = new System.Drawing.Size(473, 70);
            this.MinimumSize = new System.Drawing.Size(0, 70);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label outputLabel;
        private TestCentric.Gui.Controls.ExpandingLabel output;
    }
}
