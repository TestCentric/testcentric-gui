using System;

public class RunSummaryDisplay : Form
{
    public RunSumarryDisplay()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        // 
        // RunSummaryDisplay
        // 
        this.BackColor = System.Drawing.Color.LightYellow;
        this.ClientSize = new System.Drawing.Size(320, 60);
        this.ControlBox = false;
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RunSummaryDisplay";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.ResumeLayout(false);
    }
}
