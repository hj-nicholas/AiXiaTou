using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UL.AXT.Background.Startup))]
namespace UL.AXT.Background
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
