using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

string appSecret = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetValue("Secret", "");

if (appSecret == "")
    throw new Exception();

var key = Encoding.ASCII.GetBytes(appSecret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<apiBlog.Models.SeBlogContext>(options =>
    options.UseSqlServer("name=ConnectionStrings:Development"));

builder.Services.AddAuthorization(options => options.AddPolicy("MasterPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "M")));
builder.Services.AddAuthorization(options => options.AddPolicy("AuthorPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "A")));
builder.Services.AddAuthorization(options => options.AddPolicy("ReaderPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "R")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.UseAPIKey();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
