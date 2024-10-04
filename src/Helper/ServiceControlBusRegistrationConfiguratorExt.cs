using MassTransit;
using ServiceControl;

public static class ServiceControlBusRegistrationConfiguratorExt
{
    public static void AddServiceControl(this IBusRegistrationConfigurator x)
    {
        x.AddConfigureEndpointsCallback((context, name, cfg) =>
        {
            // TODO: context.AddRawJsonSerializer();
            cfg.UseFilter(new RetryAcknowledgementFilter());
        });
    }
}