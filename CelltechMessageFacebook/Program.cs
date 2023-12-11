using CelltechMessageFacebook.Endpoints;
using CelltechMessageFacebook.Hubs;
using CelltechMessageFacebook.Objects;
using CelltechMessageFacebook.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var appSettings = builder.Configuration.Get<AppSettings>();
services.AddSingleton(appSettings!);

services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true));
});

services.AddSignalR();
services.AddHttpClient();
services.AddScoped<IFacebookService, FacebookService>();

var app = builder.Build();
app.UseCors("AllowAllOrigins");
app.AddMessageEndpoints();
app.AddFacebookEndpoints();
app.AddUserEndpoints();

app.MapHub<ChatHub>("/hub");

app.Run();