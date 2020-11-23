using Autofac;
using Prism.Events;
using StatementAnalyticsDesktop.DataAccess;
using StatementAnalyticsDesktop.UI.Data.Lookups;
using StatementAnalyticsDesktop.UI.Data.Repositories;
using StatementAnalyticsDesktop.UI.View.Services;
using StatementAnalyticsDesktop.UI.ViewModel;

namespace StatementAnalyticsDesktop.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<StatementAnalyticsDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf().SingleInstance();
            builder.RegisterType<TransactionSearchWindowViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<StatementDetailViewModel>().As<IStatementDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<StatementRepository>().As<IStatementRepository>();
            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>();

            return builder.Build();
        }
    }
}
