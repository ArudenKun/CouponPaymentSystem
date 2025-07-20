using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Abp.Modules;
using Abp.Web;
using Castle.MicroKernel.Registration;

namespace CouponPaymentSystem;

[DependsOn(typeof(AbpWebCommonModule))]
public class SystemWebModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.IocContainer.Register(
            Component
                .For<HttpContextBase>()
                .UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current))
                .LifestylePerWebRequest(),
            // HttpContext properties
            Component
                .For<HttpRequestBase>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpContextBase>().Request)
                .LifestylePerWebRequest(),
            Component
                .For<HttpResponseBase>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpContextBase>().Response)
                .LifestylePerWebRequest(),
            Component
                .For<HttpServerUtilityBase>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpContextBase>().Server)
                .LifestylePerWebRequest(),
            Component
                .For<HttpSessionStateBase>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpContextBase>().Session)
                .LifestylePerWebRequest(),
            Component
                .For<HttpApplicationStateBase>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpContextBase>().Application)
                .LifestylePerWebRequest(),
            // HttpRequest properties
            Component
                .For<HttpBrowserCapabilitiesBase>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpRequestBase>().Browser)
                .LifestylePerWebRequest(),
            Component
                .For<HttpFileCollectionBase>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpRequestBase>().Files)
                .LifestylePerWebRequest(),
            Component
                .For<RequestContext>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpRequestBase>().RequestContext)
                .LifestylePerWebRequest(),
            // HttpResponse properties
            Component
                .For<HttpCachePolicyBase>()
                .UsingFactoryMethod(kernel => kernel.Resolve<HttpResponseBase>().Cache)
                .LifestylePerWebRequest(),
            // HostingEnvironment properties
            Component
                .For<VirtualPathProvider>()
                .UsingFactoryMethod(() => HostingEnvironment.VirtualPathProvider)
                .LifestylePerWebRequest(),
            // MVC types
            Component
                .For<UrlHelper>()
                .UsingFactoryMethod(kernel => new UrlHelper(kernel.Resolve<RequestContext>()))
                .LifestylePerWebRequest()
        );
    }
}
