using System;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class DeletePostRequest
{
    [Required]
    public string Username { get; init; }

    [Required]
    public int PostId { get; init; }
}

public class DeletePostResponse
{
    public bool DeleteIsSuccessful { get; init; }
    public bool AccessIsDenied { get; init; }
    public bool PostExist { get; set; }
}