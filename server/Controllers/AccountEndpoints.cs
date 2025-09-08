using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccountEndPoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/UserProfile", GetUserProfile);//.RequireAuthorization() or we can use Authorize Attribute on methods
            return app;
        }
     
        private static async Task<IResult> GetUserProfile(ClaimsPrincipal user,UserManager<AppUser> _userManager) 
        {
            string userID = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails =await _userManager.FindByIdAsync(userID);
            return Results.Ok(new
            {
                Email = userDetails?.Email,
                FullName=userDetails.FullName,
            });
        }
    }
    
}
