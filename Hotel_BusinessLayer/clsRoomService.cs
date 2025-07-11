﻿using Hotel_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BusinessLayer
{
    public class clsRoomService
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        public int RoomServiceID { get; private set; }
        public string RoomServiceTitle { get; set; }
        public string RoomServiceDescription { get; set; }
        public float RoomServiceFees { get; set; }

        public clsRoomService()
        {
            _Mode = enMode.AddNew;
            RoomServiceID = -1;
            RoomServiceTitle = "";
            RoomServiceDescription = "";
            RoomServiceFees = 0;
        }
        private clsRoomService(int RoomServiceID, string RoomServiceTitle, string RoomServiceDescription, float RoomServiceFees)
        {
            _Mode = enMode.Update;
            this.RoomServiceID = RoomServiceID;
            this.RoomServiceTitle = RoomServiceTitle;
            this.RoomServiceDescription = RoomServiceDescription;
            this.RoomServiceFees = RoomServiceFees;
        }

        public static clsRoomService Find(int RoomServiceID)
        {
            string RoomServiceTitle = "";
            string RoomServiceDescription = "";
            float RoomServiceFees = 0;

            bool IsFound = clsRoomServiceData.GetRoomServiceInfoByID(RoomServiceID, ref RoomServiceTitle, ref RoomServiceDescription, ref RoomServiceFees);

            if (IsFound)
                return new clsRoomService(RoomServiceID, RoomServiceTitle, RoomServiceDescription, RoomServiceFees);
            else
                return null;
        }

        public static bool IsRoomServiceExist(int RoomServiceID)
        {
            return clsRoomServiceData.IsRoomServiceExist((int)RoomServiceID);
        }

        public static bool IsRoomServiceExist(string RoomServiceTitle)
        {
            return clsRoomServiceData.IsRoomServiceExist(RoomServiceTitle);
        }

        public static Task<bool> IsRoomServiceExistAsync(int RoomServiceID)
        {
            return clsRoomServiceData.IsRoomServiceExistAsync(RoomServiceID);
        }

        public static Task<bool> IsRoomServiceExistAsync(string RoomServiceTitle)
        {
            return clsRoomServiceData.IsRoomServiceExistAsync(RoomServiceTitle);
        }

        private bool _AddNewRoomService()
        {
            RoomServiceID = clsRoomServiceData.AddNewRoomService(RoomServiceTitle, RoomServiceDescription, RoomServiceFees);
            return RoomServiceTitle != "" ;
        }

        private bool _UpdateRoomService()
        {
            return clsRoomServiceData.UpdateRoomServiceInfo(RoomServiceID, RoomServiceTitle, RoomServiceDescription, RoomServiceFees);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewRoomService())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateRoomService();

            }
            return false;
        }

        private static DataTable _roomServicesCache;
        private static readonly object _roomServicesLock = new object();

        public static DataTable GetAllRoomServices(bool forceRefresh = false)
        {
            lock (_roomServicesLock)
            {
                if (_roomServicesCache == null || forceRefresh)
                    _roomServicesCache = clsRoomServiceData.GetAllRoomServices();
                return _roomServicesCache.Copy();
            }
        }

        public static Task<DataTable> GetAllRoomServicesAsync(bool forceRefresh = false)
        {
            return Task.Run(() => GetAllRoomServices(forceRefresh));
        }

    }
}
