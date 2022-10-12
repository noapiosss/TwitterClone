using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Contracts.Database;

[Table("tbl_likes", Schema = "public")]
public class Like
{
    [Column("post_id")]
    public int LikedPostId { get; set; }
    public Post LikedPost { get; set; }

    [Column("liked_by")]
    public string LikedByUsername { get; set; }
    public User LikedBy { get; set; }
}