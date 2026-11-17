using System.Net.Http.Json;
using Library.Contracts.Books;

using var client = new HttpClient();

client.BaseAddress = new Uri("http://localhost:5000");

// 1. Lay danh sach sach
Console.WriteLine("===== DANH SACH SACH =====");

var books = await client.GetFromJsonAsync<List<BookDto>>(
    "/api/books"
);

if (books != null)
{
    foreach (var book in books)
    {
        Console.WriteLine(
            $"{book.Id} - {book.Title} - {book.Author}"
        );
    }
}

// 2. Them sach moi
Console.WriteLine();
Console.WriteLine("===== THEM SACH MOI =====");

var newBook = new CreateBookRequest(
    "Lap trinh ASP.NET Core",
    "Tran Phuong Nam",
    "ISBN004",
    2026
);

var response = await client.PostAsJsonAsync(
    "/api/books",
    newBook
);

Console.WriteLine($"Status: {(int)response.StatusCode}");

var result = await response.Content.ReadAsStringAsync();

Console.WriteLine(result);

// 3. Xem chi tiet sach vua them
Console.WriteLine();
Console.WriteLine("===== CHI TIET SACH =====");

var detail = await client.GetFromJsonAsync<BookDto>(
    "/api/books/4"
);

if (detail != null)
{
    Console.WriteLine($"{detail.Id} - {detail.Title}");
}