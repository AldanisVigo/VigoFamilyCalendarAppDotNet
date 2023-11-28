using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FamilyCalendarDotNet;
using FamilyCalendarDotNet.Models;
using FamilyCalendarDotNet.Interfaces;
using FamilyCalendarDotNet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", (corsBuilder) =>
    {
        corsBuilder
        .WithOrigins("http://localhost:5173", "http://localhost:7283")
        //.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
        //.AllowCredentials();
    });

    options.AddPolicy("Prod", (corsBuilder) =>
    {
        corsBuilder
        .WithOrigins("*","https://familycalendardotnetappservice.azurewebsites.net")
        .AllowAnyMethod()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Add MySQL Connection
builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("Local")!);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT authentication to the API
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("AppSettings:TokenKey").Value!
                )
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add JWT Token Blacklisting Service
builder.Services.AddTransient<JWTTokenBlacklistingService>();

// Add Mail Settings
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Add the email service
builder.Services.AddTransient<IMailService, MailService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("Dev");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("Prod");
    // Force HTTPS
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();

// UseAuthentication MUST come before UseAuthorization
app.UseAuthentication();

app.UseAuthorization();

// Add the JWT Token Blacklisting service middleware
app.UseMiddleware<JWTTokenBlacklistMiddleware>();

app.MapControllers();

app.Run();



