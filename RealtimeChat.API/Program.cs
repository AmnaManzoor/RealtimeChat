using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealtimeChat.Application;
using RealtimeChat.Infrastructure;
using RealtimeChat.Infrastructure.Hubs;
using RealtimeChat.Infrastructure.Data;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, config) =>
{
    config.MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Services(services);

    if (context.HostingEnvironment.IsDevelopment())
    {
        config.WriteTo.Console(new RenderedCompactJsonFormatter());
    }
    else
    {
        config.WriteTo.Console();
    }
});

builder.Services.AddSingleton(Log.Logger);

// Add services
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddSignalR();

// Add Application
builder.Services.AddApplication();

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policyBuilder =>
        {
            policyBuilder.SetIsOriginAllowed(origin =>
                origin.StartsWith("http://localhost", StringComparison.OrdinalIgnoreCase) ||
                origin.StartsWith("https://localhost", StringComparison.OrdinalIgnoreCase) ||
                origin.StartsWith("http://127.0.0.1", StringComparison.OrdinalIgnoreCase) ||
                origin.StartsWith("https://127.0.0.1", StringComparison.OrdinalIgnoreCase))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

// Add Authentication
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    // For SignalR
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/chat"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RealtimeChat API",
        Version = "v1"
    });

    // Add Security Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    await DummyDataCleanup.RemoveChatRoomsAsync(dbContext, new[]
    {
        "Product Lunch",
        "Design Guide"
    }, CancellationToken.None);
}

app.UseMiddleware<RealtimeChat.API.Middleware.ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RealtimeChat API v1");
});

var httpsPort = builder.Configuration.GetValue<int?>("HTTPS_PORT")
    ?? builder.Configuration.GetValue<int?>("ASPNETCORE_HTTPS_PORT");
if (httpsPort.HasValue)
{
    app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowLocalhost");
}

app.UseAuthentication();
app.UseAuthorization();

// Routing
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();

public partial class Program
{
}
