using Microsoft.EntityFrameworkCore;
using Realtys.Database;

namespace Realtys;

public partial class App : Application
{
    string CONNECTION_STRING = "Data source=" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Realtys.db");
    public static RealtysDbContext DbContext;


    public App()
    {
        DbContextOptions options = new DbContextOptionsBuilder().EnableSensitiveDataLogging().UseSqlite(CONNECTION_STRING).Options;
        DbContext = new RealtysDbContext(options);
        

        InitializeComponent();

        MainPage = new AppShell();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<RealtysDbContext>(options => options.UseSqlite(CONNECTION_STRING));
    }
}
