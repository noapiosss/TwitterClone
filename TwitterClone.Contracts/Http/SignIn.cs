using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Contracts.Http;

public class SignInRequest
{
    [Required]
    public string Username { get; init; }

    [Required]
    public string Password { get; init; }
}

public class SignInResponse
{
    public bool IsAuthenticated { get; set; }
    public bool UserIsFound { get; set; }
    public bool PasswordIsCorrect { get; set; }
}