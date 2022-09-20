using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class GetFollowingsRequest
{
    [Required]
    public string Username { get; init; }
}

public class GetFollowingsResponse
{
    public ICollection<string> Followings { get; init; }
}