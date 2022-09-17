using System;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class DeletePostRequest
{
    [Required]
    public int PostId { get; init; }
}

public class DeletePostResponse
{
    public bool IsDeleteSuccessful { get; init; }
}