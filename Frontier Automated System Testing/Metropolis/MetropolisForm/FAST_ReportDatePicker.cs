using CoffeeBeanLibrary;
using System;
using System.Windows.Forms;

namespace CoffeeBeanForm
{
    public partial class FAST_ReportDatePicker : Form
    {
        private FAST fast = new FAST();
        private string dailyDatedDirectory = Utility.dailyDatedDirectory;

        public FAST_ReportDatePicker()
        {
            InitializeComponent();
        }        

        private void ReportDatePicker_Load(object sender, EventArgs e)
        {
            reportDatePicker.Value = DateTime.Now;
            placeLowerRight();
        }

        private void launchReport_Click(object sender, EventArgs e)
        {
            string getDateValue = reportDatePicker.Value.ToString("dd-MMM-yyyy");
            string resFileName = dailyDatedDirectory + "\\" + getDateValue + ".html";

            try
            {
                System.Diagnostics.Process.Start(resFileName);
                this.Close();
            }
            catch (Exception)
            {
                MessageBoxEx.Show(this,"Result File Missing", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void placeLowerRight()
        {
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }
            int left = (fast.ClientSize.Width - this.Width) / 2;
            int top = (fast.ClientSize.Height - this.Height) / 2;
            int screenleft = rightmost.WorkingArea.Right - this.Width;
            int screentop = rightmost.WorkingArea.Bottom - this.Height;

            this.Left = screenleft - left;
            this.Top = screentop - top;
        }
    }
}
