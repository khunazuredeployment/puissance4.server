using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Puissance4.API.Hubs;
using Puissance4.API.Middlewares;
using Puissance4.Business.Interfaces;
using Puissance4.Business.Services;
using Puissance4.Infrastructure.Configurations;
using Puissance4.Infrastructure.Services;
using Puissance4.Persistence.Repositories;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<GameService>();

var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>();
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddScoped<JwtSecurityTokenHandler>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSingleton<IHashService, HashService>();

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IGameRepository, GameRepository>();

builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.Converters
       .Add(new JsonStringEnumConverter());
}); ;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme,
    options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = jwtConfig.ValidateIssuer,
        ValidateAudience = jwtConfig.ValidateAudience,
        ValidateLifetime = jwtConfig.ValidateLifeTime,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Signature)),
    }
);

builder.Services.AddCors(b => b.AddDefaultPolicy(options => 
    options.AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins("http://localhost:4200")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();

app.UseMiddleware<HubJwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/hubs/game");

app.Run();
