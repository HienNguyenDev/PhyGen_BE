namespace PhyGen.Api;
public static class DependencyInjection
{
       public static IServiceCollection AddPresentation(this IServiceCollection services)
       {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // REMARK: If you want to use Controllers, you'll need this.
            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                x.JsonSerializerOptions.WriteIndented = true;
            });

        services.AddProblemDetails();

            return services;
       }
}
