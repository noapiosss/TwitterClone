using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Contracts.Database;

[Table("tbl_followings", Schema = "public")]
public class Following 
{
    [Key]
    [Column("follow_by")]
    public string FollowByUsername { get; set; }
    
    [ForeignKey("FollowByUsername")]
    public User FollowBy { get; set; }

    [Key]
    [Column("follow_for")]
    public string FollowForUsername { get; set; }

    [ForeignKey("FollowForUsername")]
    public User FollowFor { get; set; }


}