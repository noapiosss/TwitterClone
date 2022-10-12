using System;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class FollowUserRequest
{
    [Required]
    public string FollowByUsername { get; init; }

    [Required]
    public string FollowForUsername { get; init; }
}

public class FollowUserResponse
{
    public bool FollowStatusIsChanged { get; init; }
}