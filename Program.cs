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
            Description = "Esta es la API de MAVE, esta API esta enfocada en ser una herramienta útil para los profesionales la salud mental, priorizando el estado anímico del usuario, también hay una función que permite encasillar a las personas en cuatro perfiles los cuales son dominante, influyente, estable y concienzudo. Tambien hay una funcionalidad que permite hacer un seguimiento de los hábitos de los usuarios, asi como también hay funciones para generar gráficas y reportes acerca de los aspectos más importantes."
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme(){
            Description = "JWT Token usar Bearer {token}",
            Name = "Authorization", 
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    },
                    Scheme= "OAuth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string> ()
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
builder.Services.AddScoped<MoodService>();
builder.Services.AddScoped<MoodRepository>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<ArticleRepository>();
builder.Services.AddScoped<ImageUtility>();
builder.Services.AddScoped<NotifyService>();
builder.Services.AddScoped<NotifyRepository>();
builder.Services.AddScoped<WhatsAppUtility>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<ReportRepository>();
builder.Configuration.AddJsonFile("appsettings.json");
var SecretKey = builder.Configuration.GetSection("Settings").GetSection("secretKey").ToString();
#pragma warning disable CS8604 // Possible null reference argument.
var Byteskey = Encoding.UTF8.GetBytes(SecretKey);
#pragma warning restore CS8604 // Possible null reference argument.

//builder.Services.AddAuthentication(config => {
//    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    config.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;
//    //config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(config => {
//    config.RequireHttpsMetadata = false;
//    config.SaveToken = true;
//    config.TokenValidationParameters = new TokenValidationParameters{
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Byteskey),
//        ValidateIssuer= false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ClockSkew= TimeSpan.Zero
//    };
//});
builder.Services.AddAuthentication(confg =>
{
    confg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    confg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Byteskey),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
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


