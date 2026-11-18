using System.Net;
using System.Net.Http.Json;
using Library.Contracts.Books;

using var client = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5000"),
    Timeout = TimeSpan.FromSeconds(10)
};

try
{
    // 1. Kiem tra trang thai API
    Console.WriteLine("===== KIEM TRA API =====");

    var healthResponse = await client.GetAsync("/");

    Console.WriteLine(
        $"HTTP Status: {(int)healthResponse.StatusCode}"
    );

    // 2. Lay danh sach sach
    Console.WriteLine();
    Console.WriteLine("===== DANH SACH SACH =====");

    var listResponse = await client.GetAsync("/api/books");

    Console.WriteLine(
        $"HTTP Status: {(int)listResponse.StatusCode}"
    );

    if (listResponse.IsSuccessStatusCode)
    {
        var books = await listResponse.Content
            .ReadFromJsonAsync<List<BookDto>>();

        if (books != null)
        {
            foreach (var book in books)
            {
                Console.WriteLine(
                    $"{book.Id} - {book.Title} - {book.Author}"
                );
            }
        }
    }

    // 3. Tim kiem sach bang query string
    Console.WriteLine();
    Console.WriteLine("===== TIM KIEM SACH JAVA =====");

    var searchResponse = await client.GetAsync(
        "/api/books?keyword=Java"
    );

    var searchBooks = await searchResponse.Content
        .ReadFromJsonAsync<List<BookDto>>();

    if (searchBooks != null)
    {
        foreach (var book in searchBooks)
        {
            Console.WriteLine(book.Title);
        }
    }

    // 4. Them sach moi
    Console.WriteLine();
    Console.WriteLine("===== THEM SACH MOI =====");

    var newBook = new CreateBookRequest(
        "Lap trinh ASP.NET Core",
        "Tran Phuong Nam",
        "ISBN004",
        2026
    );

    var createResponse = await client.PostAsJsonAsync(
        "/api/books",
        newBook
    );

    Console.WriteLine(
        $"HTTP Status: {(int)createResponse.StatusCode}"
    );

    if (createResponse.StatusCode == HttpStatusCode.Created)
    {
        var createdBook = await createResponse.Content
            .ReadFromJsonAsync<BookDto>();

        Console.WriteLine(
            $"Da them sach: {createdBook?.Title}"
        );

        Console.WriteLine(
            $"Location: {createResponse.Headers.Location}"
        );
    }
    else
    {
        var error = await createResponse.Content
            .ReadFromJsonAsync<ApiError>();

        Console.WriteLine(
            $"Ma loi: {error?.Code}"
        );

        Console.WriteLine(
            $"Thong bao: {error?.Message}"
        );
    }

    // 5. Thu lay sach khong ton tai
    Console.WriteLine();
    Console.WriteLine("===== KIEM TRA LOI 404 =====");

    var missingResponse = await client.GetAsync("/api/books/99");

    Console.WriteLine(
        $"HTTP Status: {(int)missingResponse.StatusCode}"
    );

    var missingError = await missingResponse.Content
        .ReadFromJsonAsync<ApiError>();

    Console.WriteLine(
        $"Ma loi: {missingError?.Code}"
    );

    Console.WriteLine(
        $"Thong bao: {missingError?.Message}"
    );

    // 6. Thu gui du lieu khong hop le
    Console.WriteLine();
    Console.WriteLine("===== KIEM TRA LOI 400 =====");

    var invalidBook = new CreateBookRequest(
        "",
        "Nguyen Van A",
        "ISBN005",
        2026
    );

    var invalidResponse = await client.PostAsJsonAsync(
        "/api/books",
        invalidBook
    );

    Console.WriteLine(
        $"HTTP Status: {(int)invalidResponse.StatusCode}"
    );

    var invalidError = await invalidResponse.Content
        .ReadFromJsonAsync<ApiError>();

    Console.WriteLine(
        $"Ma loi: {invalidError?.Code}"
    );

    Console.WriteLine(
        $"Thong bao: {invalidError?.Message}"
    );
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