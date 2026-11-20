namespace Library.Contracts.Reservations;

public record ReservationDto(
    Guid Id,
    int BookId,
    string StudentCode,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);