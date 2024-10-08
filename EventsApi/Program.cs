using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.UoW;
using Application.Services;
using Application.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Настройки JWT 
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton<JwtService>();

// Настройка Identity
builder.Services.AddIdentity<User, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();


// Настройка аутентификации JWT
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
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
		ClockSkew = TimeSpan.Zero
	};
});

// Добавляем роли
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
	options.AddPolicy("User", policy => policy.RequireRole("User"));
});

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme."
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

// ApplicationDbContext с InMemoryDatabase (для тестирования)
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//	options.UseInMemoryDatabase("TestDatabase")); 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(
	builder.Configuration.GetConnectionString("DefaultConnection"),
	b => b.MigrationsAssembly("Infrastructure")));


// Регистрация репозиториев
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IEventRegistrationRepository, EventRegistrationRepository>();

// Регистрация сервисов
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRegistrationService, EventRegistrationService>();
builder.Services.AddScoped<JwtService>();

// Регистрация UoW
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Регистрация AutoMapper
builder.Services.AddAutoMapper(typeof(MappingEventProfile));
builder.Services.AddAutoMapper(typeof(MappingParticipantProfile));

var app = builder.Build();

// Конфигурация pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Вызов seedata
//using (var scope = app.Services.CreateScope())
//{
//	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
//	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//	await SeedData(userManager, roleManager);
//}

app.Run();

//создание админа и юзера
//static async Task SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
//{
//	try
//    {
//        // Создание ролей
//        if (!await roleManager.RoleExistsAsync("Admin"))
//        {
//            await roleManager.CreateAsync(new IdentityRole("Admin"));
//        }

//        if (!await roleManager.RoleExistsAsync("User"))
//        {
//            await roleManager.CreateAsync(new IdentityRole("User"));
//        }

//        // Создание администратора
//        var adminUser = new User 
//        { 
//            UserName = "admin@admin.com", 
//            Email = "admin@admin.com",
//            Role = "Admin",
//            RefreshToken = GenerateRefreshToken(), 
//            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7) 
//        };

//        if (userManager.Users.All(u => u.UserName != adminUser.UserName))
//        {
//            var result = await userManager.CreateAsync(adminUser, "Admin123!");
//            if (result.Succeeded)
//            {
//                await userManager.AddToRoleAsync(adminUser, "Admin");
//            }
//            else
//            {
//                Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
//            }
//        }

//        // Создание обычного пользователя
//        var normalUser = new User 
//        { 
//            UserName = "user@user.com", 
//            Email = "user@user.com",
//            Role = "User",
//            RefreshToken = GenerateRefreshToken(),
//            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7) 
//        };

//        if (userManager.Users.All(u => u.UserName != normalUser.UserName))
//        {
//            var result = await userManager.CreateAsync(normalUser, "User123!");
//            if (result.Succeeded)
//            {
//                await userManager.AddToRoleAsync(normalUser, "User");
//            }
//            else
//            {
//                Console.WriteLine($"Error creating normal user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
//            }
//        }

//        var users = await userManager.Users.ToListAsync();
//        var roles = await roleManager.Roles.ToListAsync();

//        Console.WriteLine("Users:");
//        foreach (var user in users)
//        {
//            Console.WriteLine($"User: {user.UserName}, Email: {user.Email}, Role: {user.Role}");
//        }

//        Console.WriteLine("Roles:");
//        foreach (var role in roles)
//        {
//            Console.WriteLine($"Role: {role.Name}");
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Exception occurred: {ex.Message}");
//    }
//}

//static string GenerateRefreshToken()
//{
//    return Guid.NewGuid().ToString();
//}
