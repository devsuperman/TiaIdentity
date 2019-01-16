namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
       public static IApplicationBuilder UseTiaIdentity(this IApplicationBuilder app)
       {
            app.UseAuthentication();
            return app;
       }
    }
}