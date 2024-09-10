using Business.Interfaces;
using Business.Services;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(options
    => options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Data")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITwoFactorService, TwoFactorService>();
builder.Services.AddScoped<ILoginService, LoginService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.WithOrigins("https://localhost:7076", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
