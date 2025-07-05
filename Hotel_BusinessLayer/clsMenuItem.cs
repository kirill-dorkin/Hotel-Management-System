using Hotel_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BusinessLayer
{
    public class clsMenuItem
    {
        public enum enItemTypes { Food = 1, Beverage = 2};
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        public int ItemID { get; private set; }
        public string ItemName { get; set; }
        public enItemTypes ItemType { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public string ItemTypeText => ItemType.ToString();

        public clsMenuItem()
        {
            _Mode = enMode.AddNew;
            ItemID = -1;
            ItemName = "";
            ItemType = enItemTypes.Food;
            Price = 0;
            Description = "";
        }
        private clsMenuItem(int ItemID, string ItemName, enItemTypes ItemType, float Price, string Description,string ImagePath)
        {
            _Mode = enMode.Update;
            this.ItemID = ItemID;
            this.ItemName = ItemName;
            this.ItemType = ItemType;
            this.Price = Price;
            this.Description = Description;
            this.ImagePath = ImagePath;
        }

        public static clsMenuItem Find(int ItemID)
        {
            string ItemName = "";
            byte ItemType = 0;
            float Price = -1;
            string Description = "";
            string ImagePath = "";

            bool IsFound = clsMenuItemData.GetMenuItemInfoByID(ItemID, ref ItemName, ref ItemType, ref Price, ref Description, ref ImagePath);

            if (IsFound)
                return new clsMenuItem(ItemID, ItemName, (enItemTypes)ItemType, Price, Description,ImagePath);
            else
                return null;
        }

        public static bool IsMenuItemExist(int ItemID)
        {
            return clsMenuItemData.IsMenuItemExist(ItemID);
        }

        public static bool IsMenuItemExist(string ItemName)
        {
            return clsMenuItemData.IsMenuItemExist(ItemName);
        }

        public static Task<bool> IsMenuItemExistAsync(int ItemID)
        {
            return clsMenuItemData.IsMenuItemExistAsync(ItemID);
        }

        public static Task<bool> IsMenuItemExistAsync(string ItemName)
        {
            return clsMenuItemData.IsMenuItemExistAsync(ItemName);
        }

        private bool _AddNewMenuItem()
        {
            ItemID = clsMenuItemData.AddNewMenuItem(ItemName, (byte)ItemType, Price, Description, ImagePath);
            return ItemID != -1;
        }

        private bool _UpdateMenuItem()
        {
            return clsMenuItemData.UpdateMenuItemInfo(ItemID, ItemName, (byte)ItemType, Price, Description, ImagePath);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewMenuItem())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateMenuItem();

            }
            return false;
        }

        public static bool DeleteMenuItem(int ItemID)
        {
            return clsMenuItemData.DeleteMenuItem(ItemID);
        }

        private static DataTable _menuItemsCache;
        private static readonly object _menuItemsLock = new object();

        public static DataTable GetAllMenuItems(bool forceRefresh = false)
        {
            lock (_menuItemsLock)
            {
                if (_menuItemsCache == null || forceRefresh)
                    _menuItemsCache = clsMenuItemData.GetAllMenuItems();
                return _menuItemsCache.Copy();
            }
        }

        public static Task<DataTable> GetAllMenuItemsAsync(bool forceRefresh = false)
        {
            return Task.Run(() => GetAllMenuItems(forceRefresh));
        }
    }
}
