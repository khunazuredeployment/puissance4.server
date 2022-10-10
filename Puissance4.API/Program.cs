using Puissance4.Business.Interfaces;
using Puissance4.Business.Services;
using Puissance4.Infrastructure.Configurations;
using Puissance4.Infrastructure.Services;
using Puissance4.Persistence.Repositories;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddSingleton<GameService>();

builder.Services.AddSingleton(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>());
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSingleton<IHashService, HashService>();

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSignalR();

builder.Services.AddCors(b => b.AddDefaultPolicy(options => 
    options.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins(builder.Configuration.GetSection("AllowedHosts").Get<string>())
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
