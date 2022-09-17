using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Contracts.Database;

[Table("tbl_followings", Schema = "public")]
public class Following 
{
    [Column("follow_by")]
    public string FollowByUsername { get; set; }
    public User FollowByUser { get; set; }
    
    [Column("follow_for")]
    public string FollowForUsername { get; set; }
    public User FollowForUser { get; set; }


}