using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class GetFollowersRequest
{
    [Required]
    public string Username { get; init; }
}

public class GetFollowersResponse
{
    public ICollection<string> Followers { get; init; }
}