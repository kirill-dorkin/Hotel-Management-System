using Hotel_BusinessLayer;
using HotelManagementSystem.GlobalClasses;
using HotelManagementSystem.Reservations;
using HotelManagementSystem.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HotelManagementSystem
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
        }

        private async Task _LoadDashboardDataAsync()
        {
            // Hide greeting and user info section
            label.Visible = false;
            lblUserFullName.Visible = false;
            lblUserEmail.Visible = false;
            btnShowDropDownMenu.Visible = false;
            guna2GradientButton1.Visible = false;

            var roomsCountTask = clsRoom.GetRoomsCountAsync();
            var reservationsCountTask = clsReservation.GetReservationsCountAsync();
            var bookingsCountTask = clsBooking.GetBookingsCountAsync();
            var paymentsCountTask = clsPayment.GetPaymentsCountAsync();
            var guestsCountTask = clsGuest.GetGuestsCountAsync();
            var companionsCountTask = clsGuestCompanion.GetGuestCompanionsCountAsync();
            var usersCountTask = clsUser.GetUsersCountAsync();

            await Task.WhenAll(roomsCountTask, reservationsCountTask, bookingsCountTask,
                               paymentsCountTask, guestsCountTask, companionsCountTask,
                               usersCountTask);

            lblRoomsCount.Text = roomsCountTask.Result.ToString();
            lblReservationsCount.Text = reservationsCountTask.Result.ToString();
            lblBookingsCount.Text = bookingsCountTask.Result.ToString();
            lblPaymentsCount.Text = paymentsCountTask.Result.ToString();
            lblGuestsCount.Text = (guestsCountTask.Result + companionsCountTask.Result).ToString();
            lblUsersCount.Text = usersCountTask.Result.ToString();

        }

        private void _LoadChartData()
        {
            Series serie = chart1.Series["RoomsStatus"];

            // Ensure that the series always contains three points
            while (serie.Points.Count < 3)
                serie.Points.Add(0);

            serie.Points[0].SetValueY(clsRoom.GetRoomsCountPerStatus(clsRoom.enAvailabilityStatus.Booked));
            serie.Points[1].SetValueY(clsRoom.GetRoomsCountPerStatus(clsRoom.enAvailabilityStatus.Available));
            serie.Points[2].SetValueY(clsRoom.GetRoomsCountPerStatus(clsRoom.enAvailabilityStatus.UnderMaintenance));

            serie.Points[0].Color = ColorTranslator.FromHtml("#b676b1");
            serie.Points[1].Color = ColorTranslator.FromHtml("#fecf6a");
            serie.Points[2].Color = ColorTranslator.FromHtml("#df1c44");


        }

        private async void frmDashboard_Load(object sender, EventArgs e)
        {
            await _LoadDashboardDataAsync();

            _LoadChartData();
        }

        private void btnBookNow_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateReservation();
            frm.ShowDialog();
        }

        private void viewProfileDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowUserInfo(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void changePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangeUserPassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }
    
    }
}
