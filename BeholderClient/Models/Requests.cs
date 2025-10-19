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
    public String password_hash { get; set; }

    [JsonConstructor]
    public UserRequest(String login, String password_hash)
    {
        this.login = login;
        this.password_hash = password_hash;
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
    public Int32 channel_id { get; set; }
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
