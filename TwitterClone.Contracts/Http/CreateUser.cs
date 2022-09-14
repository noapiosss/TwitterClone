using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Contracts.Http;

public class CreateUserRequest
{
    [Required]
    [MaxLength(50)]
    public string Username { get; init; }

    [Required]
    [MaxLength(50)]
    public string Email { get; init; }
    
    [Required]
    [MaxLength(50)]
    public string Password { get; init; }
}

public class CreateUserResponse
{
    public bool IsRegistrationSuccessful { get; init; }
}