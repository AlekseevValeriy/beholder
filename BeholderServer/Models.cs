using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeholderServer.Models;

# pragma warning disable IDE1006 // Без такого формата запросы не будут работать :(
# pragma warning disable CS8618 
public class Channel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int32 id { get; set; }
    public String name { get; set; }
    public Int32 number { get; set; }
    public String? description { get; set; }
    public String? icon_path { get; set; }
    public String? tags { get; set; }
}

public class TVProgram
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int32 id { get; set; }
    public Int32 channel_id { get; set; }
    public String title { get; set; }
    public String? description { get; set; }
    public String? category { get; set; }
    public String? age_rating { get; set; }
}

public class Schedule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int32 id { get; set; }
    public Int32 program_id { get; set; }
    public Int32 channel_id { get; set; }
    public DateTime start_time { get; set; }
    public DateTime end_time { get; set; }

}

public class Favorite
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int32 id { get; set; }
    public Int32 user_id { get; set; }
    public Int32 channel_id { get; set; }
}

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int32 id { get; set; }
    public String login { get; set; }
    public String password_hash { get; set; }
}

public class UserRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(20)]
    public String login { get; set; }
    [Required]
    //[MinLength(8)]
    //[MaxLength(50)]
    public String password_hash { get; set; }
}

public class UserDeleteRequest
{
    public Int32 id { get; set; }
    [Required]
    public String login { get; set; }
    [Required]
    public String password_hash { get; set; }
}

public class IdRequest
{
    [Required]
    public Int32 id { get; set; }
}

public class ScheduleRequest
{
    [Required]
    public Int32 program_id { get; set; }
    [Required]
    public DateTime date { get; set; }
}

public class FavoriteRequest
{
    [Required]
    public Int32 channel_id { get; set; }
    [Required]
    public Int32 user_id { get; set; }
}

#pragma warning restore