using Microsoft.AspNetCore.Authorization;

namespace server.Controllers
{
    public static class AuthorizationDemoEndPoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/AdminOnly", AdminOnly);
            app.MapGet("/AdminOrTeacher", AdminOrTeacher);
            app.MapGet("/LibraryMemeberOnly", Librarymember);
            return app;
        }

        [Authorize(Policy = "AdminOnly")]
        private static string AdminOnly()
        {
            return "Admin Only";
        }

        [Authorize(Roles = "Admin,Teacher")]
        private static string AdminOrTeacher()
        {
            return "Admin Or Teacher";
        }
        [Authorize(Policy = "HasLibraryId")]
        public static string Librarymember()
        {
            return "Yeah libraryId";
        }
    }
}
