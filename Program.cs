using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TakeHomeAPI.Data;
using TakeHomeAPI.Middleware;
using TakeHomeAPI.Models;
using Serilog;
using TakeHomeAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// --- Configure Serilog for structured logging ---
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// --- Add services ---
builder.Services.AddControllers();

// --- Configure EF Core for SQL Server ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// --- API Versioning ---
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

// --- Configure JWT Authentication ---
var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

// --- Register the built-in Rate Limiter Service ---
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("global", context =>
        RateLimitPartition.GetFixedWindowLimiter("global", key =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,                          // Maximum 100 requests per window
                Window = TimeSpan.FromMinutes(1),             // Window duration of 1 minute
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0                                // No queuing
            }));
});

// --- Swagger/OpenAPI with versioning and JWT support ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Define two Swagger docs for v1 and v2
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Take Home API V1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Take Home API V2", Version = "v2" });

    // Resolve any conflicting actions by selecting the first API description
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Define the Bearer token scheme for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });

    // Include all API descriptions (for both versions) in Swagger
    c.DocInclusionPredicate((docName, apiDesc) => true);
    c.DocumentFilter<CustomTagDocumentFilter>();
    // c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
});

var app = builder.Build();

// --- Global Exception Handling Middleware ---
app.UseMiddleware<ExceptionMiddleware>();

// --- Swagger middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Take Home API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Take Home API V2");
    });

    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// --- Rate Limiting Middleware ---
// This middleware will apply the 'global' rate limiting policy to incoming requests.
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
