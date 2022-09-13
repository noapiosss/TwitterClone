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
    [Column("username")]
    public string Username { get; set; }
    
    [ForeignKey("Username")]
    public User User { get; set; }
}