using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using webapi.utilities;
using webapi.auth;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using BandManagerPWA.Services.Services;
using BandManagerPWA.Services.Interfaces;
using BandManagerPWA.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://dev-f0jsmjla2lggonlk.us.auth0.com/";
        options.Audience = $"https://bandmanager/auth0";
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = (context) =>
            {
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            },
            OnForbidden = (context) =>
            {
                // Show what the scope was that the user tried to access
                // Extract Authorization header
                var authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

                // Extract claims from User (ClaimsPrincipal)
                //var claims = context.HttpContext.User.Claims
                //    .ToDictionary(c => c.Type, c => c.Value);

                // Inspect the scope claim, if available
                var scopeClaim = context.HttpContext.User.FindFirst("scope")?.Value ?? "N/A";

                // Log or perform other diagnostic actions
                // ... your code here ...

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
var issuer = "https://dev-f0jsmjla2lggonlk.us.auth0.com/";
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("read:events", policy => policy.Requirements.Add(new HasScopeRequirement("read:events", issuer)))
    .AddPolicy("create:events", policy => policy.Requirements.Add(new HasScopeRequirement("create:events", issuer)))
    .AddPolicy("delete:events", policy => policy.Requirements.Add(new HasScopeRequirement("delete:events", issuer)))
    .AddPolicy("update:events", policy => policy.Requirements.Add(new HasScopeRequirement("update:events", issuer)))
    .AddPolicy("read:groups", policy => policy.Requirements.Add(new HasScopeRequirement("read:groups", issuer)))
    .AddPolicy("create:groups", policy => policy.Requirements.Add(new HasScopeRequirement("create:groups", issuer)))
    .AddPolicy("delete:groups", policy => policy.Requirements.Add(new HasScopeRequirement("delete:groups", issuer)))
    .AddPolicy("update:groups", policy => policy.Requirements.Add(new HasScopeRequirement("update:groups", issuer)));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BandManager API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<EventHub>("/eventHub");
app.MapControllers();

app.Run();
