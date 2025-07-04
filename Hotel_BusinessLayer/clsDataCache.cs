using System.Data;
using System.Threading.Tasks;

namespace Hotel_BusinessLayer
{
    public static class clsDataCache
    {
        public static DataTable Reservations { get; private set; }
        public static DataTable Bookings { get; private set; }

        public static async Task PreloadAsync()
        {
            Reservations = await clsReservation.GetAllReservationsAsync(true);
            Bookings = await clsBooking.GetAllBookingsAsync(true);
        }
    }
}
