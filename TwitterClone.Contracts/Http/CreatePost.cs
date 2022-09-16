using System;
using System.ComponentModel.DataAnnotations;

using TwitterClone.Contracts.Database;

namespace TwitterClone.Contracts.Http;

public class CreatePostRequest
{
    [Required]
    [MaxLength(50)]
    public string AuthorUsername { get; init; }
    
    public int? CommentTo { get; init; }
    
    [Required]
    [MaxLength(255)]
    public string Message { get; init; }
}

public class CreatePostResponse
{
    public Post Post { get; init; }
}