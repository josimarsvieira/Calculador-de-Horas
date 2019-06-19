using Calculador_de_Horas.Database;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Calculador_de_Horas
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
