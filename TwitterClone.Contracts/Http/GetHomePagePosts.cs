using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class GetHomePagePostsRequest
{
    [Required]
    public string Username { get; init; }
}

public class GetHomePagePostsResponse
{
    public ICollection<Post> PostsFromFollowings { get; init; }
}