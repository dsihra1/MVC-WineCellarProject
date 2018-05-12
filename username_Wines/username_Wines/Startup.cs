using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(username_Wines.Startup))]
namespace username_Wines
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
