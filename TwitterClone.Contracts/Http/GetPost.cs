using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class GetPostRequest
{
    [Required]
    public int PostId { get; init; }
}

public class GetPostResponse
{
    public Post Post { get; init; }
}