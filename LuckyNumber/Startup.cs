using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LuckyNumber.Startup))]
namespace LuckyNumber
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
