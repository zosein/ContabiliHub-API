using ContabiliHub.Application.Interfaces;
using ContabiliHub.Application.Services;
using ContabiliHub.Domain.Repositories;
using ContabiliHub.Infrastructure.Data;
using ContabiliHub.Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//Conexão com SQL Server

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Injeção de dependência
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//Swagger em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();


