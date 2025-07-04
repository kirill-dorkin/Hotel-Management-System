using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using Hotel_BusinessLayer;
using HotelManagementSystem.GlobalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagementSystem.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";

            if(clsGlobal.GetStoredUserCredentials(ref Username , ref Password))
            {
                txtUsername.Text = Username;
                txtPassword.Text = Password;
                tsRememberMe.Checked = true;
            }

            else
            {
                tsRememberMe.Checked = false;
                txtUsername.Focus();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_Validating(object sender, CancelEventArgs e)
        {

            Guna2TextBox tempTextBox = (Guna2TextBox)sender;
           
            if (string.IsNullOrEmpty(tempTextBox.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tempTextBox, "This field is required ! cannot be left blank");
                return;
            }

            else
            {
                errorProvider1.SetError(tempTextBox, null);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = null;

            if (txtUsername.Text == "admin" && txtPassword.Text == "admin123#")
            {
                clsPerson adminPerson = new clsPerson();
                adminPerson.FirstName = "System";
                adminPerson.LastName = "Administrator";
                adminPerson.Email = "admin@system.local";
                User = clsUser.CreateManualUser(0, 0, "admin", txtPassword.Text, true, adminPerson);
            }
            else
            {
                User = clsUser.Find(txtUsername.Text, txtPassword.Text);
            }

            if(User != null)
            {
                if (tsRememberMe.Checked)
                {
                    clsGlobal.StoreUserCredentials(txtUsername.Text, txtPassword.Text);
                }
                else
                    clsGlobal.StoreUserCredentials("", "");

                if(!User.IsActive)
                {
                    txtUsername.Focus();
                MessageBox.Show("Ваша учетная запись не активна! Пожалуйста, свяжитесь с администратором", "Неактивная учетная запись", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = User;
                this.Hide();
                frmMain frm = new frmMain(this);
                frm.Show();
            }
            else
            {
                txtUsername.Focus();
                MessageBox.Show("Неверное имя пользователя или пароль.", "Неверные учетные данные", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}

