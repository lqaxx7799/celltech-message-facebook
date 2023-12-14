using CelltechMessageFacebook.Domain;
using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects;
using CelltechMessageFacebook.Objects.RequestObjects;
using CelltechMessageFacebook.Services;
using Microsoft.AspNetCore.Mvc;

namespace CelltechMessageFacebook.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("user");

        group.MapGet("get", (
            [FromQuery] Guid id) =>
        {
            var user = DataManager.Users.GetValueOrDefault(id);
            return Results.Ok(user);
        });

        group.MapPost("signUp", async (
            [FromServices] IFacebookService facebookService,
            [FromServices] AppSettings appSettings,
            [FromBody] SignUpRequest request) =>
        {
            var accessToken = await facebookService.GenerateLongLivedToken(request.AuthResponse.AccessToken);
            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                Type = UserType.Agent,
                AccountAccessToken = accessToken.AccessToken,
                FacebookAccountId = request.AuthResponse.UserId,
                IsFacebookConnected = true,
                CreatedAt = DateTimeOffset.Now
            };
            DataManager.Users[user.Id] = user;
            foreach (var pageRequest in request.Pages)
            {
                await facebookService.SubscribeApp(pageRequest.PageId, pageRequest.AccessToken);
                var page = new FacebookPage
                {
                    Name = pageRequest.PageName,
                    PageId = pageRequest.PageId,
                    UserId = user.Id,
                    AccessToken = pageRequest.AccessToken,
                    CreatedAt = DateTimeOffset.Now
                };
                DataManager.FacebookPages[page.Id] = page;
            }
            return Results.Ok(user);
        });
    }
}