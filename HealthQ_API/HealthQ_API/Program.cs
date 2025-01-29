using System.Text;
using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<QuestionnaireService>();
builder.Services.AddScoped<PatientQuestionnaireService>();
builder.Services.AddScoped<DoctorPatientService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy1", policy =>
    {
        policy.WithOrigins("http://localhost:4200");
        policy.AllowCredentials();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };

    // Extract token from the cookie
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["auth_token"]; // Read JWT from cookies
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
}).AddCookie("Cookies", options =>
{
    options.Cookie.Name = "auth_token";
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<HealthqDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DevConnection"), 
        o => o.MapEnum<EGender>("gender").MapEnum<EUserType>("user_type"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Policy1");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();