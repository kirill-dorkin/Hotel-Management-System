using Hotel_BusinessLayer;
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
using HotelManagementSystem.GlobalClasses;

namespace HotelManagementSystem.Rooms
{
    public partial class frmListRooms : Form
    {
        private DataView _DataView;

        private string []_RoomStatusFilters =  {"Все","Свободен", "Занят", "На обслуживании" };


        public frmListRooms()
        {
            InitializeComponent();
        }

        private void _FillComboBoxWithRoomStatus()
        {
            comboBox.Items.Clear();

            foreach(string status in _RoomStatusFilters)
            {
                comboBox.Items.Add(status);
            }

        }

        private void _FillComboBoxWithRoomTypes()
        {
            comboBox.Items.Clear();

            comboBox.Items.Add("Все");

            foreach (DataRow row in clsRoomType.GetAllRoomTypes().Rows)
            {
                comboBox.Items.Add(row["Room Type Title"]);
            }

        }

        private void _FillComboBoxWithItems(string ColumnName)
        {
            if (ColumnName == "Room Type")
                _FillComboBoxWithRoomTypes();
            else
                _FillComboBoxWithRoomStatus();
        }

        private async Task _RefreshRoomsList()
        {
            clsGlobal.ShowLoading(this);
            _DataView = (await clsRoom.GetAllRoomsAsync()).DefaultView;
            dgvRoomsList.DataSource = _DataView;

            comboBox.Visible = false;
            cbFilterByOptions.SelectedIndex = 0;
            clsGlobal.HideLoading();
        }

        private void _FilterRoomsList()
        {
            if (txtFilterValue.Text.Trim() == "" || cbFilterByOptions.Text == "None")
            {
                _DataView.RowFilter = "";
                return;
            }

            _DataView.RowFilter = string.Format("[{0}] = {1}", cbFilterByOptions.Text, int.Parse(txtFilterValue.Text.Trim()));

        }

        private async void frmListRooms_Load(object sender, EventArgs e)
        {
            await _RefreshRoomsList();
        }

        private void dgvRoomsList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in dgvRoomsList.Columns)
            {
                column.Width = 150;
            }
        }

        private async void btnAddRoom_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateRoom();
            frm.ShowDialog();
            await _RefreshRoomsList();
        }

        private void cbFilterByOptions_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbFilterByOptions.Text == "Room Type" || cbFilterByOptions.Text == "Status")
            {
                txtFilterValue.Visible = false;
                _FillComboBoxWithItems(cbFilterByOptions.Text);
                comboBox.Visible = true;
                comboBox.Focus();
                comboBox.SelectedIndex = 0;
            }

            else
            {
                txtFilterValue.Visible = (cbFilterByOptions.Text != "None");
                comboBox.Visible = false;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
                txtFilterValue_TextChanged(null, EventArgs.Empty);
            }

        }
 
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.Text == "Все")
            {
                _DataView.RowFilter = null;
                return;
            }

            _DataView.RowFilter = string.Format("[{0}] = '{1}'", cbFilterByOptions.Text, comboBox.Text);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _FilterRoomsList();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int RoomID = (int) dgvRoomsList.CurrentRow.Cells[0].Value;
            Form frm = new frmShowRoomInfo(RoomID);
            frm.ShowDialog();
        }

        private async void AddRoomtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateRoom();
            frm.ShowDialog();
            await _RefreshRoomsList();

        }

        private async void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int RoomID = (int)dgvRoomsList.CurrentRow.Cells[0].Value;
            Form frm = new frmAddUpdateRoom(RoomID);
            frm.ShowDialog();
            await _RefreshRoomsList();
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int RoomID = (int)dgvRoomsList.CurrentRow.Cells[0].Value;

            if (MessageBox.Show($"Вы уверены, что хотите удалить номер с ID = {RoomID}?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if(clsRoom.DeleteRoom(RoomID))
            {
                MessageBox.Show($"Номер с ID = {RoomID} успешно удален!", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                await _RefreshRoomsList();
            }

            else
            {
                MessageBox.Show($"Не удалось удалить номер. Невозможно удалить выбранный номер из-за существующих зависимостей данных.", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void putUnderMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int RoomID = (int)dgvRoomsList.CurrentRow.Cells[0].Value;

            if (MessageBox.Show($"Вы уверены, что хотите отправить номер с ID = {RoomID} на обслуживание?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsRoom room = clsRoom.Find(RoomID); 

            if(room != null)
            {
                if (room.SetUnderMaintenance())
                {
                    MessageBox.Show($"Номер с ID = {RoomID} успешно отправлен на обслуживание!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await _RefreshRoomsList();
                }

                else
                {
                    MessageBox.Show($"Ошибка: не удалось отправить номер на обслуживание!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private async void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int RoomID = (int)dgvRoomsList.CurrentRow.Cells[0].Value;

            if (MessageBox.Show($"Вы уверены, что хотите вывести номер с ID = {RoomID} из обслуживания?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsRoom room = clsRoom.Find(RoomID);

            if (room != null)
            {
                if (room.SetAvailable())
                {
                    MessageBox.Show($"Номер с ID = {RoomID} успешно выведен из обслуживания!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await _RefreshRoomsList();
                }

                else
                {
                    MessageBox.Show($"Ошибка: не удалось вывести номер из обслуживания!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmsRooms_Opening(object sender, CancelEventArgs e)
        {
            int RoomID = (int)dgvRoomsList.CurrentRow.Cells[0].Value;

            clsRoom room = clsRoom.Find(RoomID);

            bool IsRoomAvailable = room.AvailabilityStatus == clsRoom.enAvailabilityStatus.Available;

            bool IsRoomUnderMaintenance = room.AvailabilityStatus == clsRoom.enAvailabilityStatus.UnderMaintenance; 

            putUnderMaintenanceToolStripMenuItem.Enabled = IsRoomAvailable;

            releaseToolStripMenuItem.Enabled = IsRoomUnderMaintenance;

            editToolStripMenuItem.Enabled = IsRoomAvailable || IsRoomUnderMaintenance;

            deleteToolStripMenuItem.Enabled = IsRoomAvailable || IsRoomUnderMaintenance;
        }

    }
}
