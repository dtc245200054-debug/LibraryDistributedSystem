
using Library.Contracts.Books;
using Library.Contracts.Reservations;
using Library.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

var books = new List<BookDto>
{
    new(1, "Lap trinh C#", "Nguyen Van A", "ISBN001", 2024),
    new(2, "Lap trinh Java", "Tran Van B", "ISBN002", 2023),
    new(3, "Co so du lieu", "Le Van C", "ISBN003", 2022)
};

var reservations = new List<ReservationDto>();
var reservationsLock = new object();

// 1. Kiem tra API
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        service = "Library.Api",
        status = "running"
    });
});

// 2. Lay danh sach sach
app.MapGet("/api/books", (string? keyword) =>
{
    var result = books.AsEnumerable();

    if (!string.IsNullOrWhiteSpace(keyword))
    {
        result = result.Where(b =>
            b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    return Results.Ok(result.ToList());
});

// 3. Xem chi tiet sach
app.MapGet("/api/books/{id:int}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);

    if (book is null)
    {
        return Results.NotFound(
            new ApiError("BOOK_NOT_FOUND", "Khong tim thay sach.")
        );
    }

    return Results.Ok(book);
});

// 4. Xem yeu cau dat muon
app.MapGet(
    "/api/reservations/{reservationId:guid}",
    IResult (Guid reservationId) =>
{
    ReservationDto? reservation;

    lock (reservationsLock)
    {
        reservation = reservations.FirstOrDefault(
            item => item.Id == reservationId
        );
    }

    if (reservation is null)
    {
        return Results.NotFound(
            new ApiError(
                "RESERVATION_NOT_FOUND",
                "Khong tim thay yeu cau dat muon."
            )
        );
    }

    return Results.Ok(reservation);
});

// 5. Tao yeu cau dat muon
app.MapPost(
    "/api/books/{bookId:int}/reservations",
    IResult (
        int bookId,
        CreateReservationRequest request,
        IHubContext<ReservationHub> hubContext
    ) =>
{
    var book = books.FirstOrDefault(b => b.Id == bookId);

    if (book is null)
    {
        return Results.NotFound(
            new ApiError("BOOK_NOT_FOUND", "Khong tim thay sach.")
        );
    }

    if (string.IsNullOrWhiteSpace(request.StudentCode))
    {
        return Results.BadRequest(
            new ApiError(
                "INVALID_RESERVATION",
                "Ma sinh vien khong duoc de trong."
            )
        );
    }

    var reservation = new ReservationDto(
        Guid.NewGuid(),
        bookId,
        request.StudentCode.Trim(),
        "received",
        DateTimeOffset.UtcNow,
        DateTimeOffset.UtcNow
    );

    lock (reservationsLock)
    {
        reservations.Add(reservation);
    }

    _ = SimulateReservationAsync(
        reservation.Id,
        reservations,
        reservationsLock,
        hubContext
    );

    return Results.Created(
        $"/api/reservations/{reservation.Id}",
        reservation
    );
});

// 6. Dang ky SignalR Hub
app.MapHub<ReservationHub>("/hubs/reservations");

app.Run();

// Mo phong xu ly yeu cau
static async Task SimulateReservationAsync(
    Guid reservationId,
    List<ReservationDto> reservations,
    object reservationsLock,
    IHubContext<ReservationHub> hubContext)
{
    await Task.Delay(TimeSpan.FromSeconds(1));

    await UpdateReservationStatusAsync(
        reservationId,
        "processing",
        reservations,
        reservationsLock,
        hubContext
    );

    await Task.Delay(TimeSpan.FromSeconds(2));

    await UpdateReservationStatusAsync(
        reservationId,
        "ready",
        reservations,
        reservationsLock,
        hubContext
    );
}

// Cap nhat trang thai va gui thong bao
static async Task UpdateReservationStatusAsync(
    Guid reservationId,
    string status,
    List<ReservationDto> reservations,
    object reservationsLock,
    IHubContext<ReservationHub> hubContext)
{
    ReservationDto? updatedReservation = null;

    lock (reservationsLock)
    {
        var index = reservations.FindIndex(
            item => item.Id == reservationId
        );

        if (index >= 0)
        {
            updatedReservation = reservations[index] with
            {
                Status = status,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            reservations[index] = updatedReservation;
        }
    }

    if (updatedReservation is null)
    {
        return;
    }

    var message = new ReservationStatusMessage(
        updatedReservation.Id,
        updatedReservation.Status,
        updatedReservation.UpdatedAt
    );

    await hubContext.Clients
        .Group(ReservationGroups.ForReservation(reservationId))
        .SendAsync("ReservationStatusChanged", message);
}
