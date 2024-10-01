using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using BookHub.Application.Configurations;
using BookHub.Configurations;
using BookHub.Constants;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);


builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddAutoMapperConfigurations();

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

// Configure JSON serialization to avoid circular references when converting objects to JSON.
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Registers the CORS services into the dependency injection container
builder.Services.AddCors(options =>
{
    // A CORS policy is being defined. This policy can be applied to specific endpoints or globally for the entire application
    options.AddPolicy("AllowAngularApp", configurePolicy =>
    {
        configurePolicy
            .AllowAnyOrigin() // This allows requests from any origin (any domain)
            .AllowAnyMethod() // This allows any HTTP method 
            .AllowAnyHeader(); // This allows requests to include any HTTP headers
    });
});

// Authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = AuthConstants.ExternalProviderAuthScheme;
    })
    .AddJwtBearer(x =>
    {
        // JWT configuration object
        var jwtConfig = JwtConfiguration.Create(builder.Configuration);

        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.SecretKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            ValidateLifetime = true
        };
    })
    .AddCookie(AuthConstants.ExternalProviderAuthScheme)
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        var googleAuth = builder.Configuration.GetSection("Authentication:Google");

        options.ClientId = googleAuth["ClientId"]!;
        options.ClientSecret = googleAuth["ClientSecret"]!;
    });

// Authorization
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookHub API", Version = "v1" });

    // Add support for Bearer tokens
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
            new string[] {}
        }
    });

    // Set the path to the generated XML documentation file
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // This middleware shows detailed error pages in the browser when an unhandled exception occurs.
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookHub API V1");
    });
}
else
{
    app.UseHttpsRedirection();  // Ensure HTTPS redirection in production
}

app.UseCors("AllowAngularApp");

app.UseForwardedHeaders();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();