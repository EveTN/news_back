using Autofac;
using Core.DatabaseInterfaces;
using Database.DatabaseServices;

namespace Database
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserDatabaseService>().As<IUserDatabaseService>();
        }
    }
}