using CelltechMessageFacebook.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CelltechMessageFacebook.Endpoints;

public static class HookEndpoints
{
    public static void AddHookEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("facebook/hook");
        group.MapGet("/", (
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

        group.MapPost("/", () =>
        {

        });
    }

}