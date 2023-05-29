using Microsoft.EntityFrameworkCore;

namespace SimpD.Startup;

public class RunMigrations : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder => {
            using (var scope = builder.ApplicationServices.CreateScope()) {
                var context = scope.ServiceProvider.GetService<MainContext>();
                context.Database.SetCommandTimeout(60);
                context.Database.Migrate();
            }

            next(builder);
        };
    }
}
