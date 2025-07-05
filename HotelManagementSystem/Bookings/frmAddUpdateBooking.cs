using Hotel_BusinessLayer;
using HotelManagementSystem.GlobalClasses;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace HotelManagementSystem.Bookings
{
    public partial class frmAddUpdateBooking : Form
    {
        private enum enMode { AddNew, Update };
        private enMode _Mode;
        private clsBooking _Booking;
        private int _BookingID = -1;
        private int _ReservationID = -1;

        private TextBox txtReservationID;
        private TextBox txtGuestID;
        private DateTimePicker dtpCheckIn;
        private DateTimePicker dtpCheckOut;
        private CheckBox chkCheckedOut;
        private Button btnSave;
        private Button btnClose;
        private ErrorProvider errorProvider1;

        public frmAddUpdateBooking()
        {
            _Mode = enMode.AddNew;
            _Booking = new clsBooking();
            InitializeComponent();
        }

        public frmAddUpdateBooking(int BookingID)
        {
            _Mode = enMode.Update;
            _BookingID = BookingID;
            _Booking = clsBooking.Find(BookingID);
            InitializeComponent();
        }

        private void frmAddUpdateBooking_Load(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                if (_Booking == null)
                {
                    MessageBox.Show($"Бронирование с ID = {_BookingID} не найдено!", "Не найдено", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                txtReservationID.Text = _Booking.ReservationID.ToString();
                txtGuestID.Text = _Booking.GuestID.ToString();
                dtpCheckIn.Value = _Booking.CheckInDate;
                if (_Booking.CheckOutDate.HasValue)
                    dtpCheckOut.Value = _Booking.CheckOutDate.Value;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show("Некоторые поля заполнены неверно, пожалуйста, исправьте их и попробуйте снова.", "Проверка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _Booking.ReservationID = int.Parse(txtReservationID.Text);
            _Booking.GuestID = int.Parse(txtGuestID.Text);
            _Booking.CheckInDate = dtpCheckIn.Value;
            _Booking.CheckOutDate = chkCheckedOut.Checked ? (DateTime?)dtpCheckOut.Value : null;
            _Booking.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_Booking.Save())
            {
                MessageBox.Show("Бронирование успешно сохранено", "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;
                _BookingID = _Booking.BookingID;
            }
            else
            {
                MessageBox.Show("Не удалось сохранить бронирование", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void txtReservationID_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtReservationID.Text, out _ReservationID) || !await clsReservation.IsReservationExistAsync(_ReservationID))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtReservationID, "Недействительный ID брони");
            }
            else
            {
                errorProvider1.SetError(txtReservationID, null);
            }
        }

        private void InitializeComponent()
        {
            txtReservationID = new TextBox { Left = 20, Top = 20, Width = 200 };
            txtGuestID = new TextBox { Left = 20, Top = 60, Width = 200 };
            dtpCheckIn = new DateTimePicker { Left = 20, Top = 100, Width = 200 };
            chkCheckedOut = new CheckBox { Left = 20, Top = 140, Text = "Выселен" };
            dtpCheckOut = new DateTimePicker { Left = 20, Top = 160, Width = 200 };
            btnSave = new Button { Left = 20, Top = 200, Width = 80, Text = "Сохранить" };
            btnClose = new Button { Left = 140, Top = 200, Width = 80, Text = "Закрыть" };
            errorProvider1 = new ErrorProvider();

            btnSave.Click += btnSave_Click;
            btnClose.Click += (s, e) => Close();
            txtReservationID.Validating += txtReservationID_Validating;

            Controls.Add(txtReservationID);
            Controls.Add(txtGuestID);
            Controls.Add(dtpCheckIn);
            Controls.Add(chkCheckedOut);
            Controls.Add(dtpCheckOut);
            Controls.Add(btnSave);
            Controls.Add(btnClose);

            Load += frmAddUpdateBooking_Load;

            Text = _Mode == enMode.AddNew ? "Добавить бронирование" : "Изменить бронирование";
            ClientSize = new System.Drawing.Size(260, 250);
        }
    }
}
