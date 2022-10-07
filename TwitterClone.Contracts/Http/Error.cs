namespace TwitterClone.Contracts.Http;

public enum ErrorCode
{
    BadRequest = 40000, 
    Unauthorized = 40100,
    WrongPassword = 40301,
    UserNotFound = 40401,
    PostNotFound = 40402,
    UsernameIsAlreadyInUse = 40901,
    EmailIsAlreadyInUse = 40902,
    InternalServerError = 50000,
    DbFailureError = 50001
}

public class ErrorResponse
{
    public ErrorCode Code { get; init; }
    public string Message { get; init; }
}