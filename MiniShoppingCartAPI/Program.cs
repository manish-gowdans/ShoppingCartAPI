using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniShoppingCartAPI.DbContexts;
using MiniShoppingCartAPI.Services;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    swagger.IncludeXmlComments(xmlCommentsFullPath);

    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shopping Cart API",
        Version = "v1"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the token with Bearer format"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{}
        }
    });
});

//builder.Services.AddDbContext<MiniShoppingCartApiContextSQLExpress>(dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:DBConnection"]));

builder.Services.AddDbContext<MiniShoppingCartApiContextSQLExpress>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:MiniShoppingCartConnectionForSqlServer"]);
});

builder.Services.AddScoped<IShoppingCartInfoRepository, ShoppingCartInfoRepository>();
builder.Services.AddScoped<IVerify, Verify>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretKey"]))
        };
    }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("user_type", "customer");
    });

    options.AddPolicy("MatchUserId", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
