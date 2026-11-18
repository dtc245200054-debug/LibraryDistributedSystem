using Library.Contracts.Books;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Du lieu sach mau luu trong bo nho
var books = new List<BookDto>
{
    new(1, "Lap trinh C#", "Nguyen Van A", "ISBN001", 2024),
    new(2, "Lap trinh Java", "Tran Van B", "ISBN002", 2023),
    new(3, "Co so du lieu", "Le Van C", "ISBN003", 2022)
};

// 1. Health Check: kiem tra trang thai dich vu
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        service = "Library.Api",
        status = "running"
    });
});

// 2. GET: Lay danh sach sach, ho tro tim kiem qua query string
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

// 3. GET: Xem chi tiet sach theo route parameter
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

// 4. POST: Them sach moi tu request body
app.MapPost("/api/books", (CreateBookRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Author) ||
        string.IsNullOrWhiteSpace(request.Isbn) ||
        request.PublishedYear <= 0)
    {
        return Results.BadRequest(
            new ApiError(
                "INVALID_BOOK",
                "Ten sach, tac gia, ISBN khong duoc de trong va nam xuat ban phai lon hon 0."
            )
        );
    }

    if (books.Any(b => b.Isbn.Equals(
        request.Isbn.Trim(),
        StringComparison.OrdinalIgnoreCase)))
    {
        return Results.BadRequest(
            new ApiError("DUPLICATE_ISBN", "ISBN da ton tai.")
        );
    }

    int newId = books.Count == 0 ? 1 : books.Max(b => b.Id) + 1;

    var newBook = new BookDto(
        newId,
        request.Title.Trim(),
        request.Author.Trim(),
        request.Isbn.Trim(),
        request.PublishedYear
    );

    books.Add(newBook);

    return Results.Created($"/api/books/{newId}", newBook);
});

app.Run();