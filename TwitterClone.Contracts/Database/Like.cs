using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Contracts.Database;

[Table("tbl_likes", Schema = "public")]
public class Like 
{
    [Key]
    [Column("post_id")]
    public int PostId { get; set; }

    [ForeignKey("PostId")]
    public Post Post { get; set;}

    [Key]
    [Column("liked_by_username")]
    public string LikedByUsername { get; set; }
    
    [ForeignKey("LikedByUsername")]
    public User User { get; set; }
}