using Google.Apis.Auth.AspNetCore3;
using LoginGoogleApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Add Autenticação pelo google
builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;

        o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;

        o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogleOpenIdConnect(options =>
    {
        options.ClientId = "579167870826 - 6d0ffp4123duqjocl3arv4ljkdii4huu.apps.googleusercontent.com";
        options.ClientSecret = "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";
    });


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(options => options
    .WithOrigins(new[] { "http://localhost:3000", "http://localhost:8000", "http://localhost:4200" })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
 );

app.UseAuthorization();

app.MapControllers();

app.Run();
