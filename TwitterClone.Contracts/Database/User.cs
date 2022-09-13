using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Contracts.Database;

[Table("tbl_users", Schema = "public")]
public class User 
{
    [Key]
    [MaxLength(50)]
    [Column("username")]
    public string Username { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; }

    [InverseProperty("Author")]
    public virtual List<Post> AuthoredPosts { get; set; }
    
    //[InverseProperty("LikedBy")]
    public virtual List<Like> Likes { get; set; }
    
    [InverseProperty("FollowBy")]
    public virtual List<Following> Followings { get; set; }

    [InverseProperty("FollowFor")]
    public virtual List<Following> Followers { get; set; }
    
}