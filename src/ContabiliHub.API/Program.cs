using ContabiliHub.Application.Interfaces;
using ContabiliHub.Application.Services;
using ContabiliHub.Application.Validators;
using ContabiliHub.Application.DTOs;
using ContabiliHub.Domain.Repositories;
using ContabiliHub.Infrastructure.Data;
using ContabiliHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//Conexão com SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Injeção de dependência - Repositórios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IServicoPrestadoRepository, ServicoPrestadoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

//Injeção de dependência - Serviços
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IServicoPrestadoService, ServicoPrestadoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Injeção de dependência - Validadores nativos
builder.Services.AddScoped<IValidator<ServicoPrestadoCreateDto>, ServicoPrestadoCreateDtoValidator>();
builder.Services.AddScoped<IValidator<UsuarioLoginDto>, UsuarioLoginDtoValidator>();
builder.Services.AddScoped<IValidator<UsuarioRegisterDto>, UsuarioRegisterDtoValidator>();

// Configuração do JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
        ClockSkew = TimeSpan.Zero // Remove delay padrão de 5 minutos
    };
});

builder.Services.AddAuthorization();

// Controllers e APIs
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ContabiliHub API", Version = "v1" });

    // Configuração para autenticação JWT no Swagger
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
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

// Middleware de autenticação e autorização (ordem importante!)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();


