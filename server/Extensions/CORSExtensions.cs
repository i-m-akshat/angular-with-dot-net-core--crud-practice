namespace server.Extensions
{
    public static  class CORSExtensions
    {
        static string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public static IServiceCollection AddCORS(this IServiceCollection _service)
        {
            
            return _service.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });
        }
        public static WebApplication ConfigureCORS (this WebApplication _app)
        {
          _app.UseCors(MyAllowSpecificOrigins);
            return _app;
        }
    }
}
