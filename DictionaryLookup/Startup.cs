using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DictionaryLookup.Startup))]
namespace DictionaryLookup
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
