using Microsoft.EntityFrameworkCore;
using Realtys.Database;

namespace Realtys;

public partial class App : Application
{
    public static RealtysDbContext DbContext;


    public App(RealtysDbContext dbContext)
    {
        DbContext = dbContext;
        InitializeComponent();
        MainPage = new AppShell();
    }

}
