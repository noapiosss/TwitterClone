using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class GetUserPostsRequest
{
    [Required]
    [MaxLength(50)]
    public string Username { get; init; }
}

public class GetUserPostsResponse
{
    public List<Post> UserPosts { get; init; }
}