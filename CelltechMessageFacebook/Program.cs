using CelltechMessageFacebook.Endpoints;
using CelltechMessageFacebook.Objects;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var appSettings = builder.Configuration.Get<AppSettings>();
services.AddSingleton(appSettings!);

var app = builder.Build();
app.AddMessageEndpoints();
app.AddHookEndpoints();
app.AddUserEndpoints();

app.Run();