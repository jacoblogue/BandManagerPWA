using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webapi.utilities;
using System.Security.Claims;
using webapi.auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://dev-f0jsmjla2lggonlk.us.auth0.com/";
        options.Audience = $"https://bandmanager/auth0";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:events", policy => policy.Requirements.Add(new HasScopeRequirement("read:events", $"https://dev-f0jsmjla2lggonlk.us.auth0.com/")));
    options.AddPolicy("create:events", policy => policy.Requirements.Add(new HasScopeRequirement("create:events", $"https://dev-f0jsmjla2lggonlk.us.auth0.com/")));
    options.AddPolicy("delete:events", policy => policy.Requirements.Add(new HasScopeRequirement("delete:events", $"https://dev-f0jsmjla2lggonlk.us.auth0.com/")));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<EventHub>("/eventHub");
app.MapControllers();

app.Run();
