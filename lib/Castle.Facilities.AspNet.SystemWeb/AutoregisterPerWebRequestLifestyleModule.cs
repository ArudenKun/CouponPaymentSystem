using System.Web;
using Castle.Facilities.AspNet.SystemWeb;

[assembly: PreApplicationStartMethod(typeof(PerWebRequestLifestyleModuleRegistration), "Run")]
