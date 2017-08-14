using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PaymentSchduler.Startup))]
namespace PaymentSchduler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
