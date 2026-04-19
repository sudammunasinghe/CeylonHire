using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Application.Services;
using CeylonHire.Infrastructure.Identity;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using CeylonHire.Infrastructure.Repositories;
using CeylonHire.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================================== Register Services ===================================================
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<ISqlQueryLoader, SqlQueryLoader>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobRepository, JobRepository>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

//===================================== JWT Authetication ==================================================


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();


//==================================== Swagger Configuration ==============================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//==================================== Rate Limit Configuration ==============================================

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ForgotPasswordPolicy", opt =>
    {
        opt.PermitLimit = 3; // max 3 requests
        opt.Window = TimeSpan.FromMinutes(15); // per 15 mins
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//========================================= Build App =====================================================

var app = builder.Build();

//==================================Register Global Exception =============================================

app.UseMiddleware<CeylonHire.Api.Middleware.GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
