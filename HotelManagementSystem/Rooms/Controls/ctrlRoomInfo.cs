﻿using Hotel_BusinessLayer;
using System.Windows.Forms;

namespace HotelManagementSystem.Rooms
{
    public partial class ctrlRoomInfo : UserControl
    {
        private int _RoomID = -1;

        private clsRoom _Room;

        public int RoomID
        {
            get
            {
                return _RoomID;
            }
        }

        public clsRoom Room
        {
            get
            {
                return _Room;
            }
        }

        public ctrlRoomInfo()
        {
            InitializeComponent();
        }

        public void ResetRoomInfo()
        {
            lblRoomID.Text = "[????]";
            lblRoomTypeID.Text = "[????]";
            lblRoomStatus.Text = "[????]";
            lblNotes.Text = "[????]";
            lblRoomNumber.Text = "[????]";
            lblRoomFloor.Text = "[????]";
            lblRoomSize.Text = "[????]";
            lblIsSmokingAllowed.Text = "[????]";
            lblIsPetFriendly.Text = "[????]";

            ctrlRoomTypeInfo1.ResetRoomTypeInfo();
        }

        public void LoadRoomData(int RoomID)
        {
            _Room = clsRoom.Find(RoomID);

            if (_Room == null)
            {
                MessageBox.Show($"Номер с ID = {RoomID} не найден!", "Не найдено!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetRoomInfo();
                return;
            }


            _RoomID = _Room.RoomTypeID;

            lblRoomID.Text = _Room.RoomID.ToString();
            lblRoomTypeID.Text = _Room.RoomTypeID.ToString();
            lblRoomStatus.Text = _Room.AvailabilityStatusText;
            lblNotes.Text = _Room.AdditionalNotes == "" ? "Нет заметок" : _Room.AdditionalNotes;
            lblRoomNumber.Text = _Room.RoomNumber;
            lblRoomFloor.Text = _Room.RoomFloor.ToString();
            lblRoomSize.Text = _Room.RoomSize.ToString();
            lblIsSmokingAllowed.Text = _Room.IsSmokingAllowed ? "Да" : "Нет";
            lblIsPetFriendly.Text = _Room.IsPetFriendly ? "Да" : "Нет";

            ctrlRoomTypeInfo1.LoadRoomTypeData(_Room.RoomTypeID);
        }

        public void LoadRoomData(string RoomNumber)
        {
            _Room = clsRoom.Find(RoomNumber);

            if (_Room == null)
            {
                MessageBox.Show($"Номер с номером {RoomNumber} не найден!", "Не найдено!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetRoomInfo();
                return;
            }


            _RoomID = _Room.RoomTypeID;

            lblRoomID.Text = _Room.RoomID.ToString();
            lblRoomTypeID.Text = _Room.RoomTypeID.ToString();
            lblRoomStatus.Text = _Room.AvailabilityStatusText;
            lblNotes.Text = _Room.AdditionalNotes;
            lblRoomNumber.Text = _Room.RoomNumber;
            lblRoomFloor.Text = _Room.RoomFloor.ToString();
            lblIsSmokingAllowed.Text = _Room.IsSmokingAllowed ? "Да" : "Нет";
            lblIsPetFriendly.Text = _Room.IsPetFriendly ? "Да" : "Нет";

            ctrlRoomTypeInfo1.LoadRoomTypeData(_Room.RoomTypeID);
        }

    }
}
