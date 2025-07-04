namespace HotelManagementSystem
{
    partial class frmLoading
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Timer timerAnimation;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.timerAnimation = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMessage.Location = new System.Drawing.Point(105, 90);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(82, 23);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Loading";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerAnimation
            // 
            this.timerAnimation.Interval = 500;
            this.timerAnimation.Tick += new System.EventHandler(this.timerAnimation_Tick);
            // 
            // frmLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.ControlBox = false;
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLoading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

