using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Contracts.Http;

public class CreateUserRequest
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }
}

public class CreateUserResponse
{
    public bool IsRegistrationSuccessful { get; set; }
}