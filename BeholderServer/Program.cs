using BeholderServer.Models;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static Microsoft.AspNetCore.Http.Results;

namespace BeholderServer
{
    public class Program
    {
        public static void Main(String[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TeleprogramDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddCors();

            WebApplication app = builder.Build();

            app.UseCors(p => p
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var ex = exceptionHandlerPathFeature?.Error;
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { error = ex?.Message ?? "Internal server error" });
                });
            });

            app.MapGet("/", (TeleprogramDB db) =>
            {
                return Ok();
            });

            app.MapGet("/channels", async (TeleprogramDB db) =>
            {
                var result = from channel in db.Channels
                             select new
                             {
                                 channel.id,
                                 channel.name,
                                 channel.number,
                                 channel.description,
                                 channel.icon_path,
                                 channel.tags
                             };
                if (!await result.AnyAsync())
                {
                    return NotFound();
                }
                return Ok(result);
            });

            //app.MapGet("/channels/channelId={id:int}", (Int32 id, TeleprogramDB db) =>
            app.MapGet("/channels/{id:int}", async (Int32 id, TeleprogramDB db) =>
            {
                var result = from channel in db.Channels
                             where channel.id == id
                             select new
                             {
                                 channel.id,
                                 channel.name,
                                 channel.number,
                                 channel.description,
                                 channel.icon_path,
                                 channel.tags
                             };

                if (!await result.AnyAsync())
                {
                    return NotFound();
                }
                return Ok(result);
            });

            //app.MapGet("/channels/searchQuery={searchQuery}", (String searchQuery, TeleprogramDB db) =>
            app.MapGet("/channels/search", async (String searchQuery, TeleprogramDB db) =>
            {
                var result = from channel in db.Channels
                             where channel.name.Contains(searchQuery)
                             || channel.number.ToString().Contains(searchQuery)
                             || (channel.description != null && channel.description.Contains(searchQuery))
                             || (channel.tags != null && channel.tags.Contains(searchQuery))
                             select new
                             {
                                 channel.id,
                                 channel.name,
                                 channel.number,
                                 channel.description,
                                 channel.icon_path,
                                 channel.tags
                             };

                if (!await result.AnyAsync())
                {
                    return NotFound();
                }
                return Ok(result);
            });

            app.MapPost("/schedule", async ([FromBody] ScheduleRequest request, TeleprogramDB db) =>
            {
                var result = from schedule in db.Schedule
                             join channel in db.Channels on schedule.channel_id equals channel.id
                             join program in db.Programs on schedule.program_id equals program.id
                             where schedule.program_id == request.program_id
                             && EF.Functions.DateDiffDay(schedule.start_time, request.date) == 0
                             select new
                             {
                                 program_title = program.title,
                                 channel_name = channel.name,
                                 schedule.start_time,
                                 schedule.end_time
                             };

                if (!await result.AnyAsync())
                {
                    return NotFound();
                }
                return Ok(result);
            });

            app.MapGet("/favorites/{userId:int}", async (Int32 userId, TeleprogramDB db) =>
            {
                var result = from favorite in db.Favorites
                             where favorite.user_id == userId
                             join channel in db.Channels on favorite.channel_id equals channel.id
                             select new
                             {
                                 channel.id,
                                 channel.name,
                                 channel.icon_path
                             };

                if (!await result.AnyAsync())
                {
                    return NotFound();
                }
                return Ok(result);
            });

            //app.MapGet("/favorites/channelId={id:int}", async (Int32 id, TeleprogramDB db) =>
            app.MapGet("/favorites/{userId:int}/{channelId:int}", async (Int32 userId, Int32 channelId, TeleprogramDB db) =>
            {
                var result = from favorite in db.Favorites
                             where channelId == favorite.channel_id && userId == favorite.user_id
                             select favorite;

                if (!await result.AnyAsync())
                {
                    return NotFound();
                }
                return Ok();
            });


            app.MapPut("/favorites", async ([FromBody] FavoriteRequest request, TeleprogramDB db) =>
            {
                if (await db.Favorites.AnyAsync(f => f.user_id == request.user_id && f.channel_id == request.channel_id))
                {
                    return Conflict();
                }

                if (!await db.Users.AnyAsync(u => u.id == request.user_id))
                {
                    return NotFound();
                }

                Favorite item = new Favorite()
                {
                    user_id = request.user_id,
                    channel_id = request.channel_id
                };

                await db.Favorites.AddAsync(item);

                await db.SaveChangesAsync();

                return Ok();
            });

            app.MapDelete("/favorites", async ([AsParameters] FavoriteRequest request, TeleprogramDB db) =>
            {
                var result = from favorite in db.Favorites
                             where favorite.user_id == request.user_id && favorite.channel_id == request.channel_id
                             select favorite;

                if (!await result.AnyAsync())
                {
                    return NotFound();
                }

                db.Favorites.Remove(await result.FirstAsync());

                return Ok();
            });

            app.MapGet("/users", async ([AsParameters] UserRequest request, TeleprogramDB db) =>
            {
                var result = from user in db.Users
                             where user.login == request.login && user.password_hash == request.password_hash
                             select user.id;
                if (!await result.AnyAsync())
                {
                    return NotFound();
                }

                return Ok(result);
            });

            app.MapPut("/users", async ([FromBody] UserRequest request, TeleprogramDB db) =>
            {
                if (await db.Users.AnyAsync(u => u.login == request.login))
                {
                    return Conflict();
                }

                User user = new User()
                {
                    login = request.login,
                    password_hash = request.password_hash
                };

                await db.Users.AddAsync(user);

                await db.SaveChangesAsync();

                return Ok(user.id);
            });

            app.MapDelete("/users", async ([AsParameters] UserDeleteRequest request, TeleprogramDB db) =>
            {
                var userResult = from usr in db.Users
                                 where usr.id == request.id && usr.login == request.login && usr.password_hash == request.password_hash
                                 select usr;

                if (!await userResult.AnyAsync())
                {
                    return NotFound();
                }

                User user = await userResult.FirstAsync();

                db.Users.Remove(user);

                var favoriteResult = from favorite in db.Favorites
                                     where favorite.user_id == user.id
                                     select favorite;

                if (await favoriteResult.AnyAsync())
                {
                    db.Favorites.RemoveRange(favoriteResult);
                }

                await db.SaveChangesAsync();

                return Ok();
            });

            app.Run();
        }
    }
}
