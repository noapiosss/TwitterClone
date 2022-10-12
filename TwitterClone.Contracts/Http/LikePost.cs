using System;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class LikePostRequest
{
    [Required]
    public int LikedPostId { get; init; }

    [Required]
    public string LikedByUsername { get; init; }
}

public class LikePostResponse
{
    public bool LikeStatusIsChanged { get; init; }
    public bool PostExists { get; init; }
    public bool UserExists { get; init; }
}