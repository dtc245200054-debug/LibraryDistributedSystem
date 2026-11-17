namespace Library.Contracts.Books;

public record CreateBookRequest(
    string Title,
    string Author,
    string Isbn,
    int PublishedYear
);