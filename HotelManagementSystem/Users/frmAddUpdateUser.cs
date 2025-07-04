﻿using Hotel_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagementSystem.Users
{
    public partial class frmAddUpdateUser : Form
    {
        private enum enMode { AddNew , Update};

        private enMode _Mode;

        private int _PersonID = -1;

        private int _UserID = -1;

        private clsUser _User;

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _Mode = enMode.Update;
        }

        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        private void _ResetDefaultValues()
        {
            //reset the form and its controls to their default values 

            if (_Mode == enMode.AddNew)
            {
                _User = new clsUser();
                lblTitle.Text = "Добавить пользователя";
                ctrlPersonCardWithFilter1.FilterFocus();
            }

            else
            {
                lblTitle.Text = "Обновление пользователя";
            }

            this.Text = lblTitle.Text;

            lblUserID.Text = "[????]";
            txtUserName.ResetText();
            txtPassword.ResetText();
            txtConfirmPassword.ResetText();
            chbIsActive.Checked = true;

            tpLoginInfo.Enabled = (_Mode == enMode.Update);
            btnSave.Enabled = tpLoginInfo.Enabled;

        }

        private void _LoadUserData()
        {
            //Load user information from the database 
            _User = clsUser.Find(_UserID);

            ctrlPersonCardWithFilter1.FilterEnabled = false;

            //check the if the user exists , if not close the form 
            if (_User == null)
            {
                MessageBox.Show($"Пользователь с идентификатором {_UserID} не найден!", "Не найдено", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);

            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = txtPassword.Text;
            chbIsActive.Checked = _User.IsActive;
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadUserData();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(object sender, int PersonID)
        {
            //check if the person selected exists
            if (PersonID != -1)
                _PersonID = PersonID;
        }

        private async void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "This field is required ! cannot be left blank");
                return;
            }
            //In case of mode AddNew , we check if no user has the same username as this.
            //In case of mode Update , we check if the old username was changed and if the new username is not taken
            //yet by any other user.

            else if (txtUserName.Text != _User.UserName && await clsUser.IsUserExistAsync(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "This Username is already taken by another User!");
                return;
            }

            else
            {
                errorProvider1.SetError(txtUserName, null);
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "This field is required ! cannot be left blank");
                return;
            }

            //Check if the password length is >= 8
            else if(txtPassword.Text.Trim().Length < 8)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "The password is too short ! Enter at least 8 characters.");
                return;
            }
           
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "This field is required ! cannot be left blank");
                return;
            }

            //Check if the confirmation of the password matches the password
            else if (txtPassword.Text != txtConfirmPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "The passwords do not match !");
                return;
            }

            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //check if the controls of this form are all valid 

            if (!ValidateChildren())
            {
                MessageBox.Show("Некоторые поля заполнены некорректно. Наведите курсор на красные значки, чтобы увидеть описание ошибки", "Ошибка проверки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chbIsActive.Checked;

            if(_User.Save())
            {
                MessageBox.Show("Данные пользователя успешно сохранены!", "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                lblTitle.Text = "Обновление пользователя";

                lblUserID.Text = _User.UserID.ToString();

                //change the mode to update mode
                _Mode = enMode.Update;

            }

            else
            {
                MessageBox.Show("Ошибка: данные не были сохранены.", "Сбой", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            //Check if a person is selected
            if(_PersonID == -1)
            {
                MessageBox.Show("Please select a person !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            // Check if the selected person is already assigned to another user
            else if (await clsUser.IsUserExistByPersonIDAsync(_PersonID))
            {
                MessageBox.Show("Selected person already has a user , please choose another one !", "Not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            //If you reach here then a valid person was selected successfully
            tpLoginInfo.Enabled = true;
            btnSave.Enabled = true;
            tcUserInfo.SelectedTab = tpLoginInfo;
        }

    }
}
