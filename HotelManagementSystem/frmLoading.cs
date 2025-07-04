using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class frmLoading : Form
    {
        private int _dotCount = 0;

        public frmLoading()
        {
            InitializeComponent();
            timerAnimation.Start();
        }

        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            _dotCount = (_dotCount + 1) % 4;
            lblMessage.Text = "Loading" + new string('.', _dotCount);
        }
    }
}

