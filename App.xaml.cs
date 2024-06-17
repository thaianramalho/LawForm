using System.Windows;
using LawForm.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SQLitePCL;

namespace LawForm
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Batteries.Init();
            DatabaseFacade facade = new DatabaseFacade(new DataContext());
            facade.EnsureCreated();
        }
    }
}
