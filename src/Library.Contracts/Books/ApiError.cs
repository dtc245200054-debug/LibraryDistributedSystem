namespace Library.Contracts.Books;

public record ApiError(
    string Code,
    string Message
);