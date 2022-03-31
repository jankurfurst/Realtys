using Microsoft.EntityFrameworkCore;
using Realtys.Database;

namespace Realtys;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddDbContext<RealtysDbContext>(options =>
		   options.UseSqlite("Data source=" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Realtys.db"))
		   );


		var services = builder.Services.BuildServiceProvider();
		using (var scope = services.CreateScope())
		{

			var dbContext = scope.ServiceProvider.GetRequiredService<RealtysDbContext>();
			//dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();

			DatabaseInit dbInit = new();
			dbInit.Initialization(dbContext);


		}


		return builder.Build();
	}
}
