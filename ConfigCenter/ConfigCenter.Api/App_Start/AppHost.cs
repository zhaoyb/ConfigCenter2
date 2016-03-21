using System.Web.Mvc;
using Funq;
using ServiceStack;
using ServiceStack.Mvc;

namespace ConfigCenter.Api
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("ConfigCenter", typeof(AppHost).Assembly) { }

        public override void Configure(Container container)
        {
            SetConfig(new HostConfig { HandlerFactoryPath = "api" });
            ControllerBuilder.Current.SetControllerFactory(
                new FunqControllerFactory(container));

        }
    }
}