namespace NorthwindCRUD.Helpers
{
    public static class DBSeederExtension
    {
        public static IApplicationBuilder UseSeedDB(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<DataContext>();
            DBSeeder.Seed(context);

            return app;
        }
    }
}
