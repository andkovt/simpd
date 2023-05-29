using SimpD.Service;

namespace SimpD.Startup;

public class ProvisionGui: IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder => {
            using (var scope = builder.ApplicationServices.CreateScope()) {
                var provisioner = scope.ServiceProvider.GetService<GuiProvisioner>();
                provisioner.ProvisionAsync().Wait();
            }

            next(builder);
        };
    }
}
