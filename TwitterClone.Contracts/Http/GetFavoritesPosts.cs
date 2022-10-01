using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class GetFavoritesPostsRequest
{
    [Required]
    public string Username { get; init; }
}

public class GetFavoritesPostsResponse
{
    public ICollection<Post> FavoritesPosts { get; init; }
}