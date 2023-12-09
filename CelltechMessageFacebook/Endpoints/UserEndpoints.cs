using CelltechMessageFacebook.Domain;
using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects.RequestObjects;
using Microsoft.AspNetCore.Mvc;

namespace CelltechMessageFacebook.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("user");
        group.MapPost("signUp", ([FromBody] SignUpRequest request) =>
        {
            var user = new User
            {
                Email = request.Email,
                Name = request.Username,
                Type = UserType.Agent,
            };
            DataManager.Users[user.Id] = user;
            foreach (var pageRequest in request.Pages)
            {
                var page = new FacebookPage
                {
                    Name = pageRequest.PageName,
                    PageId = pageRequest.PageId,
                    UserId = user.Id
                };
                DataManager.FacebookPages[page.Id] = page;
            }
            return Results.Ok();
        });
    }
}