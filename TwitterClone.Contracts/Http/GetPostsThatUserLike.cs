using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class GetPostsThatUserLikeRequest
{
    [Required]
    public string Username { get; init; }
}

public class GetPostsThatUserLikeResponse
{
    public ICollection<int> PostIdsThatUserLike { get; init; }
}