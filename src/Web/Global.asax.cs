using AspNet.DependencyInjection;
using DataTables.AspNet.Mvc5;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using Web;

[assembly: OwinStartup(typeof(MvcApplication))]

namespace Web;

public class MvcApplication : DependencyInjectionHttpApplication
{
    protected override void Configure(IAppBuilder app, IServiceProvider serviceProvider)
    {
        DataTableConfiguration.RegisterDataTables();
    }

    protected override void ConfigureServices(IServiceCollection services) { }
}
