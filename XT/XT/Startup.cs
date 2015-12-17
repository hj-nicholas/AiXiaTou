using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XT.Startup))]
namespace XT
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
