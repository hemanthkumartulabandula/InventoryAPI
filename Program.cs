using InventoryAPI.Data;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json.Serialization;
using InventoryAPI.Hubs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];


//builder.Services.AddDbContext<InventoryContext>(options =>
 //   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

 var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
 string connectionString;

 if (!string.IsNullOrEmpty(databaseUrl))
 {
     var uri = new Uri(databaseUrl);
     var userInfo = uri.UserInfo.Split(':');

     var host = uri.Host;
     var port = uri.Port == -1 ? 5432 : uri.Port; // ✅ FIX: use default port 5432 if missing

     connectionString =
         $"Host={host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
 }

 else
 {
     connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
 }


 Console.WriteLine("👉 Using DB connection: " + connectionString);

 Console.WriteLine($"✅ Final connection string being used: {connectionString}");

 builder.Services.AddDbContext<InventoryContext>(options =>
     options.UseNpgsql(connectionString)
         .LogTo(Console.WriteLine, LogLevel.Information)); 

 //builder.Services.AddDbContext<InventoryContext>(options =>
   //  options.UseNpgsql(connectionString));




builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<InventoryContext>()
    .AddDefaultTokenProviders();


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
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200", 
                "https://verdant-shortbread-0eaeab.netlify.app"
                ) 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
});



builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSignalR();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and your valid JWT token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API V1");
    c.RoutePrefix = "";
});


app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapHub<InventoryHub>("/hubs/inventory");


app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var context = services.GetRequiredService<InventoryContext>();
    
    var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

    if (pendingMigrations.Any())
    {
        Console.WriteLine("📦 Applying EF Core migrations...");
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Migrations applied.");
    }
    else
    {
        Console.WriteLine("✔️ No pending migrations. Database is up-to-date.");
    }


    if (!context.Products.Any())
    {
        await DataSeeder.SeedSampleDataAsync(context);
    }

    await RoleSeeder.SeedRolesAndAdminAsync(services);
}

app.Run();
