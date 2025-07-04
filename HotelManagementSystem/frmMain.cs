using Guna.UI2.WinForms;
using Hotel_BusinessLayer;
using HotelManagementSystem.Bookings;
using HotelManagementSystem.GlobalClasses;
using HotelManagementSystem.Guests;
using HotelManagementSystem.Login;
using HotelManagementSystem.MenuItems;
using HotelManagementSystem.Orders;
using HotelManagementSystem.Payments;
using HotelManagementSystem.People;
using HotelManagementSystem.Properties;
using HotelManagementSystem.Reservations;
using HotelManagementSystem.Rooms;
using HotelManagementSystem.Rooms.RoomServices;
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

namespace HotelManagementSystem
{
    public partial class frmMain : Form
    {
        private frmLogin _frmLogin;

        private frmDashboard _frmDashboard;
        private frmListReservations _frmReservations;
        private frmListBookings _frmBookings;
        private frmListRooms _frmRooms;
        private frmListRoomTypes _frmRoomTypes;
        private frmListRoomServices _frmRoomServices;
        private frmListGuests _frmGuests;
        private frmListUsers _frmUsers;
        private frmListPayments _frmPayments;
        private MenuItems.frmListMenuItems _frmMenuItems;
        private frmListGuestOrders _frmGuestOrders;

        private static T _EnsureForm<T>(ref T form) where T : Form, new()
        {
            if (form == null || form.IsDisposed)
                form = new T();

            return form;
        }
        public frmMain(frmLogin LoginForm)
        {
            InitializeComponent();
            _frmLogin = LoginForm;
        }

        private void _FillFormInPanelContainer(Form frm)
        {
            frm.BackColor = panelContainer.FillColor;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
     
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(frm);

            frm.Show();

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmDashboard));

        }

        private void btnReservations_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmReservations));
        }

        private void btnBookings_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmBookings));

        }

        private void btnRooms_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmRooms));
        }

        private void btnRoomTypes_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmRoomTypes));
        }

        private void btnRoomServices_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmRoomServices));
        }

        private void btnGuests_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmGuests));
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmUsers));
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmPayments));
        }

        private void btnDiningMenu_Click(object sender, EventArgs e)
        {
            _FillFormInPanelContainer(_EnsureForm(ref _frmMenuItems));

        }

        private void btnGuestOrders_Click(object sender, EventArgs e)
        {
           _FillFormInPanelContainer(_EnsureForm(ref _frmGuestOrders));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            clsGlobal.CurrentUser = null;
            _frmLogin.Show();
        }

        private async void frmMain_Load(object sender, EventArgs e)
        {
            await clsDataCache.PreloadAsync();
            _EnsureForm(ref _frmDashboard);
            _EnsureForm(ref _frmReservations);
            _EnsureForm(ref _frmBookings);
            _EnsureForm(ref _frmRooms);
            _EnsureForm(ref _frmRoomTypes);
            _EnsureForm(ref _frmRoomServices);
            _EnsureForm(ref _frmGuests);
            _EnsureForm(ref _frmUsers);
            _EnsureForm(ref _frmPayments);
            _EnsureForm(ref _frmMenuItems);
            _EnsureForm(ref _frmGuestOrders);
            btnDashboard.PerformClick();
        }
    }
}
