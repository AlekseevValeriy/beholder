using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Beholder.Models;
public class UserRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(20)]
    public String login { get; set; }
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public String password { get; set; }

    [JsonConstructor]
    public UserRequest(String login, String password)
    {
        this.login = login;
        this.password = password;
    }
}

public class IdRequest
{
    [Required]
    public Int32 id { get; set; }
}

public class ScheduleRequest
{
    [Required]
    public Int32 id { get; set; }
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
