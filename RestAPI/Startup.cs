using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestAPI.Models;
using RestAPI.Services;

namespace RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CalcHoursDatabaseSettings>(Configuration.GetSection(nameof(CalcHoursDatabaseSettings)));

            services.AddSingleton<ICalcHours>(sp =>
            sp.GetRequiredService<IOptions<CalcHoursDatabaseSettings>>().Value)
                .AddSingleton<BankHoursService>()
                .AddSingleton<CompanyService>()
                .AddSingleton<EmployeeHoursService>()
                .AddSingleton<EmployeeService>();

            services.AddControllers();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}