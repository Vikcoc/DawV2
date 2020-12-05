using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DawV2.Startup))]
namespace DawV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
