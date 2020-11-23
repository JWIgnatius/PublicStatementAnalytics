using System.Linq;
using System.Windows;
using Autofac;
using Microsoft.EntityFrameworkCore;
using StatementAnalyticsDesktop.DataAccess;
using StatementAnalyticsDesktop.UI.Startup;
using StatementAnalytics.Helpers;

namespace StatementAnalyticsDesktop.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            using (var ctx = new StatementAnalyticsDbContext())
            {
                var path = @"C:\Users\User\Documents\Bank Statements\Both";
                var statements = Helper.GetAllStatementsFromFolder(path, datesOnly: true);

                var dbStatements = ctx.Statements.AsNoTracking().Select(s=>new
                {
                    s.StatementDate, s.Bank
                }).ToList();

                foreach (var statement in statements)
                {
                    if (!dbStatements.Contains(new {statement.StatementDate, statement.Bank}))
                    {
                        ctx.Add(Helper.GetStatement(statement.FilePath));
                    }
                }

                ctx.SaveChanges();
            }

            var mainWindow = container.Resolve<MainWindow>();

            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Unexpected error occurred. Please inform Joe & Ellie." +
                $"\n{e.Exception.Message}, Unexpected error");
            e.Handled = true;
        }
    }
}
