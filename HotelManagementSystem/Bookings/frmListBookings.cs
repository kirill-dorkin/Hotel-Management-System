﻿using Hotel_BusinessLayer;
using HotelManagementSystem.GlobalClasses;
using HotelManagementSystem.Guests;
using HotelManagementSystem.Reservations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagementSystem.Bookings
{
    public partial class frmListBookings : Form
    {
        private DataView _DataView;

        public frmListBookings()
        {
            InitializeComponent();
        }

        private async Task _RefreshBookingsList()
        {
            clsGlobal.ShowLoading(this);
            _DataView = (await clsBooking.GetAllBookingsAsync()).DefaultView;
            dgvBookingsList.DataSource = _DataView;

            cbStatus.Visible = false;
            cbFilterByOptions.SelectedIndex = 0;
            clsGlobal.HideLoading();
        }

        private void _FilterBookingsList()
        {
            if (txtFilterValue.Text.Trim() == "" || cbFilterByOptions.Text == "Нет")
            {
                _DataView.RowFilter = "";
                return;
            }

            if (cbFilterByOptions.Text != "Гость")
                _DataView.RowFilter = string.Format("[{0}] = {1}", cbFilterByOptions.Text, int.Parse(txtFilterValue.Text.Trim()));
            else
                _DataView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", cbFilterByOptions.Text, txtFilterValue.Text.Trim());
        }

        private async void frmListBookings_Load(object sender, EventArgs e)
        {
            await _RefreshBookingsList();
        }

        private void dgvBookingsList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in dgvBookingsList.Columns)
            {
                column.Width = 200;
            }
        }

        private void cbFilterByOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterByOptions.Text == "Статус")
            {
                txtFilterValue.Visible = false;
                cbStatus.Visible = true;
                cbStatus.Focus();
                cbStatus.SelectedIndex = 0;
            }

            else
            {
                txtFilterValue.Visible = (cbFilterByOptions.Text != "Нет");
                cbStatus.Visible = false;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
                txtFilterValue_TextChanged(null, EventArgs.Empty);
            }
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatus.Text == "Все")
            {
                _DataView.RowFilter = null;
                return;
            }

            _DataView.RowFilter = string.Format("[{0}] = '{1}'", cbFilterByOptions.Text, cbStatus.Text);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _FilterBookingsList();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterByOptions.Text != "Гость")
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void cmsReservations_Opening(object sender, CancelEventArgs e)
        {
            int BookingID = (int)dgvBookingsList.CurrentRow.Cells[0].Value;
         
            clsBooking Booking = clsBooking.Find(BookingID);

            ShowGuestCompanionsToolStripMenuItem.Enabled = Booking.ReservationInfo.NumberOfPeople > 1;

            checkOutToolStripMenuItem.Enabled = Booking.Status == clsBooking.enStatus.Ongoing;

            editToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;

        }

        private async void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int BookingID = (int)dgvBookingsList.CurrentRow.Cells[0].Value;

            clsBooking Booking = clsBooking.Find(BookingID);

            if (Booking != null)
            {
                if (Booking.CheckOut(clsGlobal.CurrentUser.UserID))
                {
                    MessageBox.Show("Выселение прошло успешно", "Выселение успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await _RefreshBookingsList();
                }
                else
                {
                    MessageBox.Show("Ошибка при выселении.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showDetailsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int BookingID = (int)dgvBookingsList.CurrentRow.Cells[0].Value;

            Form frm = new frmShowBookingInfo(BookingID);
            frm.ShowDialog();
        }

        private void ShowGuestCompanionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int BookingID = (int)dgvBookingsList.CurrentRow.Cells[0].Value;

            Form frm = new frmShowGuestCompanions(BookingID);
            frm.ShowDialog();
        }

        private async void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateBooking();
            frm.ShowDialog();
            await _RefreshBookingsList();
        }

        private async void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int BookingID = (int)dgvBookingsList.CurrentRow.Cells[0].Value;
            Form frm = new frmAddUpdateBooking(BookingID);
            frm.ShowDialog();
            await _RefreshBookingsList();
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить это бронирование?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int BookingID = (int)dgvBookingsList.CurrentRow.Cells[0].Value;

            if (clsBooking.DeleteBooking(BookingID))
            {
                MessageBox.Show("Бронирование успешно удалено", "Удалено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await _RefreshBookingsList();
            }
            else
            {
                MessageBox.Show("Бронирование не удалено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
    }
}
