namespace CoffeeBeanForm
{
    partial class FAST_ReportDatePicker
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
            this.reportDatePicker = new System.Windows.Forms.DateTimePicker();
            this.launchReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // reportDatePicker
            // 
            this.reportDatePicker.Location = new System.Drawing.Point(13, 13);
            this.reportDatePicker.Name = "reportDatePicker";
            this.reportDatePicker.Size = new System.Drawing.Size(230, 20);
            this.reportDatePicker.TabIndex = 0;
            this.reportDatePicker.Value = new System.DateTime(2016, 5, 3, 11, 25, 35, 0);
            // 
            // launchReport
            // 
            this.launchReport.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.launchReport.Location = new System.Drawing.Point(86, 39);
            this.launchReport.Name = "launchReport";
            this.launchReport.Size = new System.Drawing.Size(75, 23);
            this.launchReport.TabIndex = 1;
            this.launchReport.Text = "Launch Report";
            this.launchReport.UseVisualStyleBackColor = true;
            this.launchReport.Click += new System.EventHandler(this.launchReport_Click);
            // 
            // FAST_ReportDatePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 66);
            this.Controls.Add(this.launchReport);
            this.Controls.Add(this.reportDatePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FAST_ReportDatePicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select A Report Date";
            this.Load += new System.EventHandler(this.ReportDatePicker_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker reportDatePicker;
        private System.Windows.Forms.Button launchReport;
    }
}