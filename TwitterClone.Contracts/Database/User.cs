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
    [MaxLength(256)]
    [Column("password")]
    public string Password { get; set; }

    public ICollection<Post> Posts { get; set; }

    public ICollection<Like> Likes { get; set; }

    public ICollection<Following> Followings { get; set; }

    public ICollection<Following> Followers { get; set; }

}