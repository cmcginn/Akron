using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Akron.Web.Startup))]
namespace Akron.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
