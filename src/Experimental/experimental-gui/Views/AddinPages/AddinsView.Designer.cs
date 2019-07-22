namespace TestCentric.Gui.Views.AddinPages
{
    partial class AddinsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddinsView));
            this.availablePoints = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // availablePoints
            // 
            this.availablePoints.AutoScroll = true;
            this.availablePoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.availablePoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.availablePoints.Location = new System.Drawing.Point(3, 3);
            this.availablePoints.Margin = new System.Windows.Forms.Padding(0);
            this.availablePoints.Name = "availablePoints";
            this.availablePoints.Size = new System.Drawing.Size(478, 356);
            this.availablePoints.TabIndex = 0;
            // 
            // AddinsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.availablePoints);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddinsView";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddinsView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel availablePoints;
    }
}