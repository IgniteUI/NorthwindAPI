namespace NorthwindCRUD.Helpers
{
    public static class DBSeederExtension
    {
        public static IApplicationBuilder UseSeedDB(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                DBSeeder.Seed(context);
            }
            catch (Exception ex)
            {
                //TODO log error
            }

            return app;
        }
    }
}
