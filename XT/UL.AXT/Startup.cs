using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UL.AXT.Startup))]
namespace UL.AXT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
