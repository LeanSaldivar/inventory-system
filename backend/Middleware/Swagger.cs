namespace backend.middleware;

public static class Swagger
{
     public static WebApplication SwaggerDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        return app;
    }
}
