using System.Text;
using MAVE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MAVE.Repositories;
using MAVE.Utilities;
using MAVE.Services;
using System.ComponentModel;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>{
        options.SwaggerDoc("v1", new OpenApiInfo{
            Title = "Mave API",
            Version = "v1",
            Description = "Api para la gestion de usuarios y procesos para el seguimiento del estado animico"
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme(){
            Name = "Autorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    }
);
builder.Services.AddScoped<UserRepositories>();
builder.Services.AddScoped<QuestionRepository>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<TokenAndEncipt>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmailUtility>();
builder.Services.AddScoped<EvaluationUtility>();
builder.Configuration.AddJsonFile("appsettings.json");
var SecretKey = builder.Configuration.GetSection("Settings").GetSection("SecretKey").ToString();
#pragma warning disable CS8604 // Possible null reference argument.
var Byteskey = Encoding.UTF8.GetBytes(SecretKey);
#pragma warning restore CS8604 // Possible null reference argument.

builder.Services.AddAuthentication(config => {
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;
    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config => {
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Byteskey),
        ValidateIssuer= false,
        ValidateAudience = false
    };
});


builder.Services.AddControllers();
builder.Services.AddDbContext<DbAa60a4MavetestContext>(op=>op.UseSqlServer(builder.Configuration.GetConnectionString("Base")));
builder.Services.AddTransient<DbAa60a4MavetestContext>();

builder.Services.AddCors(options=>{
    options.AddPolicy ("NuevaPolitica", app=>{
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


