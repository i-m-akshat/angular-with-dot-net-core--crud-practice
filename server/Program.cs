using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using server.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
string conString = builder.Configuration.GetConnectionString("DevDB");
//builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//services from identity core
builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conString));

//CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});//this is much more flexible than app.useCors


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
//app.UseCors(options =>
//{
//    options.WithOrigins("http://localhost:4200/");
//});////you can use this one or you can use builder one
app.MapGroup("/Account")
    .MapIdentityApi<AppUser>();
app.MapPost("/Account/signup",async(
    UserManager<AppUser> userManager,[FromBody]UserRegistrationModel registrationModel
    ) =>
{
    AppUser user = new AppUser()
    {
        Email=registrationModel.Email,
        FullName=registrationModel.FullName,
      UserName=registrationModel.Email,
    };
    var res=await userManager.CreateAsync(user, registrationModel.Password);

    if (res.Succeeded)
    {
        return Results.Ok(res);
    }
    else
    {
        return Results.BadRequest(res);
    }
});
app.UseHttpsRedirection();


app.Run();



public class UserRegistrationModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }

}