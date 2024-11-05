using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.UoW;
using Infrastructure;
using Infrastructure.Services;
using Application.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Application.DTOs;
using Application.Validators;
using FluentValidation;
using Application.UseCase;
using Application.UseCases;
using Application.Interfaces;


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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader();
		});
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
		Description = "Введите токен JWT Bearer"
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

// Регистрация UseCase
builder.Services.AddScoped<AddEventRegistrationUseCase>();
builder.Services.AddScoped<AddEventUseCase>();
builder.Services.AddScoped<AddImageUrlToEventUseCase>();
builder.Services.AddScoped<AddParticipantUseCase>();
builder.Services.AddScoped<DeleteEventUseCase>();
builder.Services.AddScoped<DeleteParticipantUseCase>();
builder.Services.AddScoped<GetAllEventsUseCase>();
builder.Services.AddScoped<GetAllParticipantsUseCase>();
builder.Services.AddScoped<GetEventByIdUseCase>();
builder.Services.AddScoped<GetEventsByCriteriesUseCase>();
builder.Services.AddScoped<GetParticipantByIdUseCase>();
builder.Services.AddScoped<GetParticipantsByEventIdUseCase>();
builder.Services.AddScoped<IsEmailUniqueUseCase>();
builder.Services.AddScoped<IsTitleUniqueUseCase>();
builder.Services.AddScoped<RemoveEventRegistrationUseCase>();
builder.Services.AddScoped<UpdateEventUseCase>();
builder.Services.AddScoped<UpdateParticipantUseCase>();
builder.Services.AddScoped<LoginUseCase>();


// Регистрация UoW
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Регистрация AutoMapper
builder.Services.AddAutoMapper(typeof(MappingEventProfile));
builder.Services.AddAutoMapper(typeof(MappingParticipantProfile));

// Регистрация Validators
builder.Services.AddTransient<IValidator<EventDto>, EventDtoValidator>();
builder.Services.AddTransient<IValidator<ParticipantDto>, ParticipantDtoValidator>();


var app = builder.Build();

// Конфигурация pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
//	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//	await SeedData(userManager, roleManager);
//}

app.Run();

//static async Task SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
//{
//	string[] roles = { "Admin", "User" };
//	foreach (var role in roles)
//	{
//		if (!await roleManager.RoleExistsAsync(role))
//		{
//			var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
//			if (!roleResult.Succeeded)
//			{
//				Console.WriteLine($"Ошибка при создании роли {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
//				return;
//			}
//		}
//	}

//	var adminEmail = "admin@admin.com";
//	var userEmail = "user@user.com";

//	var adminUser = await userManager.FindByEmailAsync(adminEmail);
//	if (adminUser == null)
//	{
//		adminUser = new User { UserName = adminEmail, Email = adminEmail, Role = "Admin" };
//		var result = await userManager.CreateAsync(adminUser, "Admin123!");
//		if (result.Succeeded)
//		{
//			await userManager.AddToRoleAsync(adminUser, "Admin");
//		}
//		else
//		{
//			Console.WriteLine($"Ошибка создания администратора: {string.Join(", ", result.Errors.Select(e => e.Description))}");
//		}
//	}

//	var normalUser = await userManager.FindByEmailAsync(userEmail);
//	if (normalUser == null)
//	{
//		normalUser = new User { UserName = userEmail, Email = userEmail, Role = "User" };
//		var result = await userManager.CreateAsync(normalUser, "User123!");
//		if (result.Succeeded)
//		{
//			await userManager.AddToRoleAsync(normalUser, "User");
//		}
//		else
//		{
//			Console.WriteLine($"Ошибка создания пользователя: {string.Join(", ", result.Errors.Select(e => e.Description))}");
//		}
//	}
//}


