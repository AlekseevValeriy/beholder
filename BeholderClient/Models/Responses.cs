using System.Text.Json.Serialization;

namespace Beholder.Models;
# pragma warning disable IDE1006 // Без такого формата запросы не будут работать :(
# pragma warning disable CS8618 
public class ChannelResponse
{
    public static ChannelResponse Empty => new(0, "", 0, "", "", "");
    public Int32 id { get; set; }
    public String name { get; set; }
    public Int32 number { get; set; }
    public String? description { get; set; }
    public String? icon_path { get; set; }
    public String? tags { get; set; }

    [JsonConstructor]
    public ChannelResponse(Int32 id, String name, Int32 number, String? description, String? icon_path, String? tags)
    {
        this.id = id;
        this.name = name;
        this.number = number;
        this.description = description;
        this.icon_path = icon_path;
        this.tags = tags;
    }
}

public class ScheduleResponse
{
    public String program_title { get; set; }
    public String channel_name { get; set; }
    public DateTime start_time { get; set; }
    public DateTime end_time { get; set; }

    [JsonConstructor]
    public ScheduleResponse(String program_title, String channel_name, DateTime start_time, DateTime end_time)
    {
        this.program_title = program_title;
        this.channel_name = channel_name;
        this.start_time = start_time;
        this.end_time = end_time;
    }
}

public class ScheduleResponseGroup : ObservableCollection<ScheduleResponse>
{
    public String Name { get; private set; }

    public ScheduleResponseGroup(String name, ObservableCollection<ScheduleResponse> items) : base(items)
    {
        Name = name;
    }
}

public class FavoriteResponse
{
    public Int32 id { get; set; }
    public String name { get; set; }
    public String icon_path { get; set; }

    [JsonConstructor]
    public FavoriteResponse(Int32 id, String name, Int32 number, String description, String icon_path)
    {
        this.id = id;
        this.name = name;
        this.icon_path = icon_path;
    }
}

public class UserResponse
{
    public Int32 id { get; set; }

    [JsonConstructor]
    public UserResponse(Int32 id)
    {
        this.id = id;
    }
}

public class UserCreateResponse
{
    public static UserCreateResponse Empty => new(0, "", "");
    public Int32 id { get; set; }
    public String login { get; set; }
    public String password { get; set; }

    [JsonConstructor]
    public UserCreateResponse(Int32 id, String login, String password)
    {
        this.id = id;
        this.login = login;
        this.password = password;
    }
}

#pragma warning restore