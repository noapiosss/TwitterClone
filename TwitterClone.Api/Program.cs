using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using TwitterClone.Api.Configuration;
using TwitterClone.Domain.Commands;
using TwitterClone.Domain.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppConfiguration>(builder.Configuration);

builder.Services.AddMediatR(typeof(CreateUserCommand));
builder.Services.AddDbContext<TwitterCloneDbContext>((sp, options) => 
{
    var configuration = sp.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
    options.UseNpgsql(configuration.CurrentValue.ConnectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();
app.MapControllers();

app.Run();
