using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class GetLikesRequest
{
    [Required]
    public int PostId { get; init; }
}

public class GetLikesResponse
{
    public ICollection<string> UsersThatLikePost { get; init; }
}