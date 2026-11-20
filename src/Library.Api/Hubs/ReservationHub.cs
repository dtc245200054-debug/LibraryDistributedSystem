using Microsoft.AspNetCore.SignalR;

namespace Library.Api.Hubs;

public class ReservationHub : Hub
{
    public Task WatchReservation(string reservationId)
    {
        return Groups.AddToGroupAsync(
            Context.ConnectionId,
            ReservationGroups.ForReservation(reservationId)
        );
    }
}