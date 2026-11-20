
namespace Library.Api.Hubs;

public static class ReservationGroups
{
    public static string ForReservation(Guid reservationId)
    {
        return ForReservation(reservationId.ToString());
    }

    public static string ForReservation(string reservationId)
    {
        return $"reservation:{reservationId}";
    }
}
