
using System.Net;
using System.Net.Http.Json;
using Library.Contracts.Books;
using Library.Contracts.Reservations;
using Microsoft.AspNetCore.SignalR.Client;

var apiBaseUrl = args.Length > 0
    ? args[0].TrimEnd('/')
    : "http://localhost:5000";

using var http = new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl),
    Timeout = TimeSpan.FromSeconds(10)
};

try
{
    // 1. Ket noi SignalR
    await using var connection = new HubConnectionBuilder()
        .WithUrl($"{apiBaseUrl}/hubs/reservations")
        .WithAutomaticReconnect()
        .Build();

    var completedNotification = new TaskCompletionSource<bool>();

    // 2. Dang ky nhan thong bao
    connection.On<ReservationStatusMessage>(
        "ReservationStatusChanged",
        message =>
        {
            Console.WriteLine();
            Console.WriteLine("===== THONG BAO SIGNALR =====");
            Console.WriteLine($"Ma yeu cau: {message.ReservationId}");
            Console.WriteLine($"Trang thai: {message.Status}");
            Console.WriteLine($"Thoi gian: {message.UpdatedAt:HH:mm:ss}");

            if (message.Status == "ready")
            {
                completedNotification.TrySetResult(true);
            }
        }
    );

    // 3. Bat dau ket noi
    await connection.StartAsync();

    Console.WriteLine("Da ket noi SignalR thanh cong.");

    // 4. Lay danh sach sach
    Console.WriteLine();
    Console.WriteLine("===== DANH SACH SACH =====");

    var books = await http.GetFromJsonAsync<List<BookDto>>(
        "/api/books"
    );

    if (books is null || books.Count == 0)
    {
        Console.WriteLine("Khong co sach.");
        return;
    }

    foreach (var book in books)
    {
        Console.WriteLine(
            $"{book.Id} - {book.Title} - {book.Author}"
        );
    }

    // 5. Chon sach dau tien
    var selectedBook = books.First();

    Console.WriteLine();
    Console.WriteLine($"Dang dat muon sach: {selectedBook.Title}");

    // 6. Tao yeu cau dat muon bang REST
    var request = new CreateReservationRequest(
        "DTC245200054"
    );

    var response = await http.PostAsJsonAsync(
        $"/api/books/{selectedBook.Id}/reservations",
        request
    );

    Console.WriteLine($"HTTP Status: {(int)response.StatusCode}");

    if (response.StatusCode != HttpStatusCode.Created)
    {
        Console.WriteLine("Khong tao duoc yeu cau dat muon.");
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        return;
    }

    var createdReservation =
        await response.Content.ReadFromJsonAsync<ReservationDto>();

    if (createdReservation is null)
    {
        Console.WriteLine("Khong doc duoc thong tin yeu cau.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== TAO YEU CAU THANH CONG =====");
    Console.WriteLine($"Ma yeu cau: {createdReservation.Id}");
    Console.WriteLine($"Trang thai ban dau: {createdReservation.Status}");

    // 7. Dang ky theo doi yeu cau
    await connection.InvokeAsync(
        "WatchReservation",
        createdReservation.Id.ToString()
    );

    Console.WriteLine();
    Console.WriteLine("Dang cho thong bao tu may chu...");

    // 8. Cho thong bao ready
    var completed = await Task.WhenAny(
        completedNotification.Task,
        Task.Delay(TimeSpan.FromSeconds(10))
    );

    if (completed != completedNotification.Task)
    {
        Console.WriteLine("Qua thoi gian cho thong bao.");
    }

    // 9. Doc lai trang thai bang REST
    Console.WriteLine();
    Console.WriteLine("===== KIEM TRA TRANG THAI CUOI =====");

    var latestReservation =
        await http.GetFromJsonAsync<ReservationDto>(
            $"/api/reservations/{createdReservation.Id}"
        );

    Console.WriteLine($"Trang thai cuoi: {latestReservation?.Status}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine("Khong ket noi duoc den API.");
    Console.WriteLine(ex.Message);
}
catch (TaskCanceledException)
{
    Console.WriteLine("Yeu cau da qua thoi gian cho.");
}
catch (Exception ex)
{
    Console.WriteLine("Co loi xay ra:");
    Console.WriteLine(ex.Message);
}
