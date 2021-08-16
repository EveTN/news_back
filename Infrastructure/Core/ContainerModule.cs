using Autofac;
using Core.Services.Auth;
using Models.DomainInterfaces;

namespace Core
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorizationService>().As<IAuthorizationService>();
        }
    }
}