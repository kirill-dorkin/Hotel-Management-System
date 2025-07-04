﻿using Hotel_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BusinessLayer
{
    public class clsGuest
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        public int GuestID { get; private set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsPerson PersonInfo { get; }

        public clsGuest()
        {
            _Mode = enMode.AddNew;
            GuestID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
        }
        private clsGuest(int GuestID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            _Mode = enMode.Update;
            this.GuestID = GuestID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
            PersonInfo = clsPerson.Find(PersonID);
        }

        public static clsGuest Find(int GuestID)
        {
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            bool IsFound = clsGuestData.GetGuestInfoByID(GuestID, ref PersonID, ref CreatedByUserID, ref CreatedDate);

            if (IsFound)
                return new clsGuest(GuestID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;
        }

        public static clsGuest FindByPersonID(int PersonID)
        {
            int GuestID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            bool IsFound = clsGuestData.GetGuestInfoByPersonID(PersonID, ref GuestID, ref CreatedByUserID, ref CreatedDate);

            if (IsFound)
                return new clsGuest(GuestID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;
        }

        public static bool IsGuestExist(int GuestID)
        {
            return clsGuestData.IsGuestExist(GuestID);
        }

        public static bool IsGuestExistByPersonID(int PersonID)
        {
            return clsGuestData.IsGuestExistByPersonID(PersonID);
        }

        private bool _AddNewGuest()
        {
            GuestID = clsGuestData.AddNewGuest(PersonID, CreatedByUserID, CreatedDate);
            return GuestID != -1;
        }

        private bool _UpdateGuest()
        {
            return clsGuestData.UpdateGuestInfo(GuestID, PersonID, CreatedByUserID, CreatedDate);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewGuest())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateGuest();

            }
            return false;
        }

        public static bool DeleteGuest(int GuestID)
        {
            return clsGuestData.DeleteGuest(GuestID);
        }

        private static DataTable _guestsCache;
        private static readonly object _guestsLock = new object();

        public static DataTable GetAllGuests(bool forceRefresh = false)
        {
            lock (_guestsLock)
            {
                if (_guestsCache == null || forceRefresh)
                    _guestsCache = clsGuestData.GetAllGuests();
                return _guestsCache.Copy();
            }
        }

        public static Task<DataTable> GetAllGuestsAsync(bool forceRefresh = false)
        {
            return GetAllGuestsAsyncInternal(forceRefresh);
        }

        private static async Task<DataTable> GetAllGuestsAsyncInternal(bool forceRefresh)
        {
            lock (_guestsLock)
            {
                if (_guestsCache != null && !forceRefresh)
                    return _guestsCache.Copy();
            }

            var data = await clsGuestData.GetAllGuestsAsync();

            lock (_guestsLock)
            {
                _guestsCache = data;
                return _guestsCache.Copy();
            }
        }

        public static DataTable GetAllGuestCompanions(int GuestID , int BookingID)
        {
            return clsGuestData.GetAllGuestCompanions(GuestID , BookingID);
        }

        public static int GetGuestsCount()
        {
            return clsGuestData.GetGuestsCount();
        }

        public static Task<int> GetGuestsCountAsync()
        {
            return clsGuestData.GetGuestsCountAsync();
        }

    }
}
