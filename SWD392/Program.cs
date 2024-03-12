using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Repos;
using Services.Interface;
using Services.Services;
using System.Text;
using Services.Extensions;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<ASPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ASPDB")));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITagRepository, TagRepository>();
builder.Services.AddTransient<IReservationRepository, ReservationRepository>();
builder.Services.AddTransient<IReportRepository, ReportRepository>();
builder.Services.AddTransient<IReportCauseRepository, ReportCauseRepository>();
builder.Services.AddTransient<IRatingRepository, RatingRepository>();
builder.Services.AddTransient<IPackageRepository, PackageRepository>();
builder.Services.AddTransient<IArtworkTypeRepository, ArtworkTypeRepository>();
builder.Services.AddTransient<IArtworkRepository, ArtworkRepository>();
builder.Services.AddTransient<IActiveSubscriptionRepository, ActiveSubscriptionRepository>();
builder.Services.AddTransient<IAzureStorageRepository, AzureStorageRepository>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IReservationService, ReservationService>();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddTransient<IRatingService, RatingService>();
builder.Services.AddTransient<IPackageService, PackageService>();
builder.Services.AddTransient<IArtworkService, ArtworkService>();
builder.Services.AddTransient<IAzureService, AzureService>();
/*builder.Services.AddSingleton<TokenService>();*/
builder.Services.AddSingleton<Services.Extensions.TokenService>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(typeof(Program).Assembly);   
builder.Services.AddJwtAuthenticationService(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerService();

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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseStaticFiles();
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
