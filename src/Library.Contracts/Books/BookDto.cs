namespace Library.Contracts.Books;

public record BookDto(
    int Id,
    string Title,
    string Author,
    string Isbn,
    int PublishedYear
);