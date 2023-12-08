using AuctionService.Consumers;
using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//looks for any class that derives from Profile
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//MASS TRANSIT CONFIG
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AuctionDbContext>(x =>
    {

        x.QueryDelay = TimeSpan.FromSeconds(10);
        x.UsePostgres();
        x.UseBusOutbox();
    });

    x.AddConsumersFromNamespaceContaining<ActionCreatedFaultConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);


    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => {
            opt.Authority = builder.Configuration["IdentityServiceUrl"];
            opt.RequireHttpsMetadata = false;
            opt.TokenValidationParameters.ValidateAudience = false;
            opt.TokenValidationParameters.NameClaimType= "username";
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
//authentication has to come before auhthorization and must config addAuthentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    //pass web applicaton
    DbInitializer.InitDb(app);
}
catch (Exception ex)
{

    Console.WriteLine(ex);
}

app.Run();
