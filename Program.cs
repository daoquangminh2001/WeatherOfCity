using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WeatherOfCity;
using WeatherOfCity.Models;
using WeatherOfCity.Sevices;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<WeatherContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("constring")));

//config cho cache
var redisConf = new RedisConfiguration();
builder.Configuration.GetSection("RedisConfiguration").Bind(redisConf);
builder.Services.AddSingleton(redisConf);
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConf.RedisConnectionString));
builder.Services.AddStackExchangeRedisCache(option => option.Configuration = redisConf.RedisConnectionString);
builder.Services.AddSingleton<IResponseCacheServices, ResponseCacheServices>();

builder.Services.AddEndpointsApiExplorer();

/*builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("oath", new OpenApiSecurityScheme {
    Description = "nhập token vào đây ,hehe (\"bearer {token}\")",
    In = ParameterLocation.Header,
    Name = "Xác thực người dùng",
    Type = SecuritySchemeType.ApiKey
    });
    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});*/
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddScoped<WeatherContext, WeatherContext>();
builder.Services.AddScoped<ICityRespornsitory, CityRespornsitory>();
builder.Services.AddScoped<IWeatherCity, WeatherCity>();
builder.Services.AddScoped<IUserSevices, UserSevices>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SecretKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,   
        };

    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin",
        authBuiler => {
            authBuiler.RequireRole("admin");
        });
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
         context.Response.StatusCode = StatusCodes.Status404NotFound;
        /*await context.Response.WriteAsync("hehe");*/
    });
});


app.Run();
