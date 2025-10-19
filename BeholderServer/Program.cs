using Azure.Core;

using BeholderServer.Models;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static Microsoft.AspNetCore.Http.Results;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                var query = from channel in db.Channels
                             select new
                             {
                                 channel.id,
                                 channel.name,
                                 channel.number,
                                 channel.description,
                                 channel.icon_path,
                                 channel.tags
                             };
                var result = await query.AsNoTracking().ToListAsync();

                if (result is null || result.Count == 0)
                {
                    return NotFound();
                }
                return Ok(result);
            });

            app.MapGet("/channels/{id:int}", async (Int32 id, TeleprogramDB db) =>
            {
                var query = from channel in db.Channels
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
                var result = await query.AsNoTracking().ToListAsync();

                if (result is null || result.Count == 0)
                {
                    return NotFound();
                }
                return Ok(result);
            });

            app.MapGet("/channels/search", async (String searchQuery, TeleprogramDB db) =>
            {
                var query = from channel in db.Channels
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
                var result = await query.AsNoTracking().ToListAsync();

                if (result is null || result.Count == 0)
                {
                    return NotFound();
                }
                return Ok(result);
            });

            app.MapGet("/schedule/{channelId:int}/{requestDate}", async (Int32 channelId, String requestDate, TeleprogramDB db) =>
            {
                if (!DateTime.TryParse(requestDate, out DateTime date))
                {
                    return Conflict();
                }

                DateTime startDate = date.Date;
                DateTime endDate = startDate.AddDays(1);

                var query = from schedule in db.Schedule
                            join channel in db.Channels on schedule.channel_id equals channel.id
                            join program in db.Programs on schedule.program_id equals program.id
                            where schedule.channel_id == channelId
                            && schedule.start_time >= startDate
                            && schedule.end_time <= endDate
                            select schedule;

                var result = await query.TagWith("OPTION (RECOMPILE)").AsNoTracking().ToListAsync();

                if (result is null || result.Count == 0)
                {
                    return NotFound();
                }
                return Ok();
            });

            app.MapPost("/schedule", async ([FromBody] ScheduleRequest request, TeleprogramDB db) =>
            {
                DateTime startDate = request.date.Date;
                DateTime endDate = startDate.AddDays(1);

                //String sqlQuery = @"
                //    SELECT 
                //        T3.title AS program_title, 
                //        T2.name AS channel_name, 
                //        T1.start_time, 
                //        T1.end_time
                //    FROM 
                //        Schedule T1 WITH (NOLOCK)
                //        JOIN Channels T2 WITH (NOLOCK) ON T1.channel_id = T2.id
                //        JOIN Programs T3 WITH (NOLOCK) ON T1.program_id = T3.id
                //    WHERE 
                //        T1.channel_id = {0}
                //        AND T1.start_time >= {1}
                //        AND T1.start_time < {2}";

                //var result = await db.Set<ScheduleItemDto>()
                //    //.FromSqlRaw(sqlQuery)
                //    .FromSqlRaw(sqlQuery, request.channel_id, startDate, endDate)
                //    .TagWith("OPTION (RECOMPILE)")
                //    .AsNoTracking()
                //    .ToListAsync();

                var query = from schedule in db.Schedule
                            join channel in db.Channels on schedule.channel_id equals channel.id
                            join program in db.Programs on schedule.program_id equals program.id
                            where schedule.channel_id == request.channel_id
                            && schedule.start_time >= startDate
                            && schedule.end_time <= endDate
                            select new
                            {
                                program_title = program.title,
                                program_description = program.description,
                                program_category = program.category,
                                program_age_rating = program.age_rating,
                                schedule.start_time,
                                schedule.end_time
                            };

                var result = await query.TagWith("OPTION (RECOMPILE)").AsNoTracking().ToListAsync();

                if (result is null || result.Count == 0)
                {
                    return NotFound();
                }
                return Ok(result);
            });

            app.MapGet("/favorites/{userId:int}", async (Int32 userId, TeleprogramDB db) =>
            {
                var query = from favorite in db.Favorites
                             where favorite.user_id == userId
                             join channel in db.Channels on favorite.channel_id equals channel.id
                             select new
                             {
                                 channel.id,
                                 channel.name,
                                 channel.icon_path
                             };
                var result = await query.AsNoTracking().ToListAsync();

                if (result is null || result.Count == 0)
                {
                    return NotFound();
                }
                return Ok(result);
            });

            //app.MapGet("/favorites/channelId={id:int}", async (Int32 id, TeleprogramDB db) =>
            app.MapGet("/favorites/{userId:int}/{channelId:int}", async (Int32 userId, Int32 channelId, TeleprogramDB db) =>
            {
                var query = from favorite in db.Favorites
                             where channelId == favorite.channel_id && userId == favorite.user_id
                             select favorite;
                var result = await query.AsNoTracking().ToListAsync();

                if (result is null || result.Count == 0)
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
                var query = from favorite in db.Favorites
                             where favorite.user_id == request.user_id && favorite.channel_id == request.channel_id
                             select favorite;
                var result = await query.ToListAsync();

                if (result is null || result.Count == 0)
                {
                    return NotFound();
                }

                db.Favorites.Remove(result.First());

                await db.SaveChangesAsync();

                return Ok();
            });

            app.MapGet("/users", async ([AsParameters] UserRequest request, TeleprogramDB db) =>
            {
                var query = from user in db.Users
                             where user.login == request.login && user.password_hash == request.password_hash
                             select user.id;
                var result = await query.ToListAsync();

                if (result is null || result.Count == 0)
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
                var userQuery = from usr in db.Users
                                 where usr.id == request.id && usr.login == request.login && usr.password_hash == request.password_hash
                                 select usr;
                var userResult = await userQuery.ToListAsync();

                if (userResult is null || userResult.Count == 0)
                {
                    return NotFound();
                }

                User user = userResult.First();

                db.Users.Remove(user);

                var favoriteQuery= from favorite in db.Favorites
                                     where favorite.user_id == user.id
                                     select favorite;
                var favoriteResult = await favoriteQuery.ToListAsync();

                if (favoriteResult is not null && favoriteResult.Count != 0)
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
