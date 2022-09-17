using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Contracts.Database;

[Table("tbl_posts", Schema = "public")]
public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("post_id")]
    public int PostId { get; set; }

    [Column("author")]
    public string AuthorUsername { get; set; }
    public User Author {get;set;}

    [Column("comment_to")]
    public int? CommentTo { get; set; }

    [Column("post_date")]
    public DateTime PostDate { get; set; }
    
    [Required]
    [MaxLength(256)]
    [Column("message")]
    public string Message { get; set; }
    
    public ICollection<Like> Likes { get; set; }
    
}