namespace server.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerExplorer(this IServiceCollection _service)
        {
            _service.AddEndpointsApiExplorer();
            _service.AddSwaggerGen();
            return _service;
        }
        public static WebApplication ConfigureSwaggerExplorer(this WebApplication _app)
        {
            if (_app.Environment.IsDevelopment())
            {
                _app.UseSwagger();
                _app.UseSwaggerUI();
                //_app.MapOpenApi();
            }
            return _app;
        }
    }
}
