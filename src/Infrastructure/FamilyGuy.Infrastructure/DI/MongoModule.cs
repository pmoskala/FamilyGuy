using Autofac;
using FamilyGuy.Infrastructure.Infrastructure;
using FamilyGuy.Infrastructure.Mongo;
using MongoDB.Driver;
using System.Reflection;

namespace FamilyGuy.Infrastructure.DI
{
    public class MongoModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) =>
            {
                MongoSettings settings = c.Resolve<MongoSettings>();

                return new MongoClient(settings.ConnectionString);
            }).SingleInstance();

            builder.Register((c, p) =>
            {
                MongoClient mongoClient = c.Resolve<MongoClient>();
                MongoSettings settings = c.Resolve<MongoSettings>();
                IMongoDatabase database = mongoClient.GetDatabase(settings.Database);

                return database;
            }).As<IMongoDatabase>();

            Assembly assembly = typeof(MongoModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsAssignableTo<IMongoRepository>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
