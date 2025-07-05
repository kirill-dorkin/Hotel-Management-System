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

        public static async Task PreloadAsync(bool forceRefresh = false)
        {
            if (!forceRefresh &&
                Reservations != null &&
                Bookings != null &&
                Rooms != null &&
                RoomTypes != null &&
                RoomServices != null &&
                Guests != null &&
                Users != null &&
                Payments != null &&
                MenuItems != null)
                return;

            var reservationsTask = clsReservation.GetAllReservationsAsync(true);
            var bookingsTask = clsBooking.GetAllBookingsAsync(true);
            var roomsTask = clsRoom.GetAllRoomsAsync(true);
            var roomTypesTask = clsRoomType.GetAllRoomTypesAsync(true);
            var roomServicesTask = clsRoomService.GetAllRoomServicesAsync(true);
            var guestsTask = clsGuest.GetAllGuestsAsync(true);
            var usersTask = clsUser.GetAllUsersAsync(true);
            var paymentsTask = clsPayment.GetAllPaymentsAsync(true);
            var menuItemsTask = clsMenuItem.GetAllMenuItemsAsync(true);

            await Task.WhenAll(reservationsTask, bookingsTask, roomsTask, roomTypesTask,
                               roomServicesTask, guestsTask, usersTask, paymentsTask,
                               menuItemsTask);

            Reservations = reservationsTask.Result;
            Bookings = bookingsTask.Result;
            Rooms = roomsTask.Result;
            RoomTypes = roomTypesTask.Result;
            RoomServices = roomServicesTask.Result;
            Guests = guestsTask.Result;
            Users = usersTask.Result;
            Payments = paymentsTask.Result;
            MenuItems = menuItemsTask.Result;
        }
    }
}
