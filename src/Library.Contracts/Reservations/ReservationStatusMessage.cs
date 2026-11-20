namespace Library.Contracts.Reservations;

public record ReservationStatusMessage(
    Guid ReservationId,
    string Status,
    DateTimeOffset UpdatedAt
);