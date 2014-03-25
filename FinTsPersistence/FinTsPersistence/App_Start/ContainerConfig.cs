using Autofac;
using FinTsPersistence.Actions;
using FinTsPersistence.TanSources;

namespace FinTsPersistence.App_Start
{
    /// <summary>
    /// Wires up Autofac
    /// </summary>
    public static class ContainerConfig
    {
        private static IContainer container;

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ActionBalance>()
                   .As<IAction>()
                   .WithMetadata(ActionFactory.ActionName, ActionBalance.ActionName);

            builder.RegisterType<ActionStatement>()
                   .As<IAction>()
                   .WithMetadata(ActionFactory.ActionName, ActionStatement.ActionName);

            builder.RegisterType<ActionStatement>()
                   .As<IAction>()
                   .WithMetadata(ActionFactory.ActionName, ActionRemittDebit.ActionName);

            builder.RegisterType<ActionXml>()
                   .As<IAction>()
                   .WithMetadata(ActionFactory.ActionName, ActionXml.ActionName);

            builder.RegisterType<ActionSepa>()
                   .As<IAction>()
                   .WithMetadata(ActionFactory.ActionName, ActionSepa.ActionName);

            builder.RegisterType<ActionFactory>()
                    .As<IActionFactory>();

            builder.RegisterType<TanSourceFactory>()
                    .As<ITanSourceFactory>();

            builder.RegisterType<FinTsPersistence>()
                   .As<IFinTsPersistence>();

            //perform auto-wiring
            ////builder.RegisterAssemblyTypes(programAssembly).AsImplementedInterfaces();


            return builder.Build();
        }

        /// <summary>
        /// Retrieve a service from the context.
        /// </summary>
        public static T Resolve<T>()
        {
            if (container == null)
            {
                container = BuildContainer();
            }

            return container.Resolve<T>();
        }
    }
}