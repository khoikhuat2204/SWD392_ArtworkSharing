using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Repos;
using Services.Interface;
using Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ASPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ASPDB")));

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ASPContext>();
        context.Database.Migrate(); // Apply pending migrations
    }
    catch (Exception ex)
    {
        throw new Exception(ex.InnerException?.ToString());
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.MapControllers();

app.Run();
