using System.Data;
using System.Threading.Tasks;

namespace Hotel_BusinessLayer
{
    public static class clsDataCache
    {
        public static DataTable Reservations { get; private set; }
        public static DataTable Bookings { get; private set; }
        public static DataTable Rooms { get; private set; }
        public static DataTable RoomTypes { get; private set; }
        public static DataTable RoomServices { get; private set; }
        public static DataTable Guests { get; private set; }
        public static DataTable Users { get; private set; }
        public static DataTable Payments { get; private set; }
        public static DataTable MenuItems { get; private set; }

        public static async Task PreloadAsync()
        {
            Reservations = await clsReservation.GetAllReservationsAsync(true);
            Bookings = await clsBooking.GetAllBookingsAsync(true);
            Rooms = await clsRoom.GetAllRoomsAsync(true);
            RoomTypes = await clsRoomType.GetAllRoomTypesAsync(true);
            RoomServices = await clsRoomService.GetAllRoomServicesAsync(true);
            Guests = await clsGuest.GetAllGuestsAsync(true);
            Users = await clsUser.GetAllUsersAsync(true);
            Payments = await clsPayment.GetAllPaymentsAsync(true);
            MenuItems = await clsMenuItem.GetAllMenuItemsAsync(true);
        }
    }
}
