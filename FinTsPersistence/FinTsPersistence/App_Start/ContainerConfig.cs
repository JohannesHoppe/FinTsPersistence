using System.Configuration;
using Autofac;
using FinTsPersistence.Actions;
using FinTsPersistence.Code;
using FinTsPersistence.Code.ConsoleInputOutput;
using FinTsPersistence.Code.DbLoggerInputOutput;
using FinTsPersistence.Model;
using FinTsPersistence.TanSources;

namespace FinTsPersistence
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

            builder.RegisterType<CommandLineHelper>()
                    .As<ICommandLineHelper>();

            builder.RegisterType<TransactionContext>()
                   .As<ITransactionContext>();

            if (ConfigurationManager.AppSettings["WriteOutputToDb"].ToUpper() == "TRUE") {

                builder.RegisterType<DbLogger>()
                        .As<IInputOutput>();
            }   else {

                builder.RegisterType<Console>()
                       .As<IInputOutput>();
            }

            builder.RegisterType<TransactionRepository>()
                   .As<ITransactionRepository>();

            builder.RegisterType<Date>()
                    .As<IDate>();

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

            builder.RegisterType<ActionPersist>()
                   .As<IAction>()
                   .WithMetadata(ActionFactory.ActionName, ActionPersist.ActionName);

            builder.RegisterType<ActionFactory>()
                    .As<IActionFactory>();

            builder.RegisterType<TanSourceFactory>()
                    .As<ITanSourceFactory>();

            builder.RegisterType<FinTsService>()
                   .As<IFinTsService>();

            builder.RegisterType<LazyTransactionService>()
                    .As<ITransactionService>();

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