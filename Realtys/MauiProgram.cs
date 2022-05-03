using Microsoft.EntityFrameworkCore;
using Realtys.Database;
using Realtys.Models;
using Realtys.ViewModels;
using Realtys.Views;

namespace Realtys;

public static class MauiProgram
{
	private static string PATH = Path.Combine
		(
			Environment.GetFolderPath
				(
					Environment.SpecialFolder.LocalApplicationData
				), 
			"Realtys.db"
		);

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
		   options.UseSqlite("Data source=" + PATH)
		   );

		builder.Services.AddTransient<EditCreateViewModel>();
		builder.Services.AddTransient<EntryPage>();
		
		builder.Services.AddSingleton<ListPage>();


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
