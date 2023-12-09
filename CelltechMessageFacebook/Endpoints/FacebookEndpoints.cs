using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects;
using CelltechMessageFacebook.Services;
using Microsoft.AspNetCore.Mvc;

namespace CelltechMessageFacebook.Endpoints;

public static class FacebookEndpoints
{
    public static void AddFacebookEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("facebook");
        
        // hook - start
        group.MapGet("hook", (
            [FromServices] AppSettings appSettings,
            [FromQuery(Name = "hub.mode")] string mode = "",
            [FromQuery(Name = "hub.challenge")] string challenge = "",
            [FromQuery(Name = "hub.verify_token")] string verifyToken = "") =>
        {
            if (verifyToken != appSettings.Facebook.WebhookVerifyCode)
            {
                return Results.BadRequest("Verify code not matched");
            }
            int.TryParse(challenge, out var result);
            return Results.Ok(result);
        });

        group.MapPost("hook", ([FromBody] object payload) =>
        {
            ;
        });
        
        // hook - end

        group.MapGet("pages", async ([FromServices] IFacebookService facebookService, [FromQuery] string accessToken) =>
        {
            var pages = await facebookService.GetPages(accessToken);
            return Results.Ok(pages);
        });
    }

}