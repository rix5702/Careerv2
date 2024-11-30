using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CareerProject.StartupOwin))]

namespace CareerProject
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
