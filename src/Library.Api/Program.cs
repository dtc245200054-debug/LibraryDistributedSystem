using Library.Contracts.Books;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Du lieu sach luu trong bo nho
var books = new List<BookDto>
{
    new BookDto(1, "Lap trinh C#", "Nguyen Van A", "ISBN001", 2024),
    new BookDto(2, "Lap trinh Java", "Tran Van B", "ISBN002", 2023),
    new BookDto(3, "Co so du lieu", "Le Van C", "ISBN003", 2022)
};

// API 1: Lay danh sach sach
app.MapGet("/api/books", () =>
{
    return Results.Ok(books);
});

// API 2: Lay thong tin sach theo ID
app.MapGet("/api/books/{id:int}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);

    if (book == null)
    {
        return Results.NotFound(new
        {
            message = "Khong tim thay sach."
        });
    }

    return Results.Ok(book);
});

// API 3: Them sach moi
app.MapPost("/api/books", (CreateBookRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Author) ||
        string.IsNullOrWhiteSpace(request.Isbn) ||
        request.PublishedYear <= 0)
    {
        return Results.BadRequest(new
        {
            message = "Thong tin sach khong hop le."
        });
    }

    if (books.Any(b => b.Isbn == request.Isbn))
    {
        return Results.BadRequest(new
        {
            message = "ISBN da ton tai."
        });
    }

    int newId = books.Count == 0
        ? 1
        : books.Max(b => b.Id) + 1;

    var newBook = new BookDto(
        newId,
        request.Title,
        request.Author,
        request.Isbn,
        request.PublishedYear
    );

    books.Add(newBook);

    return Results.Created($"/api/books/{newId}", newBook);
});

app.Run();