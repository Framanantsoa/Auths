using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Utils;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Token JWT. Exemple: Bearer eyJhbGciOiJIUzI1...",
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
});

// CONNECTION TO DATABASE
builder.Services.AddDbContext<DbaContext>(op => op.UseNpgsql(
    builder.Configuration.GetConnectionString("Default")
));

// CONTROLLERS AND JSON FORMATS
builder.Services.AddControllers(options => {
    options.Filters.Add<ValidationFilter>();
})
.ConfigureApiBehaviorOptions(options => {
    options.SuppressModelStateInvalidFilter = true;
});

// CORS 
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Cors1", policy =>
    {
        policy.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

// AUTO-INJECTION DES DEPENDANCES
builder.Services.AddProjectServices();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors("Cors1");
app.MapControllers();

app.Run();
