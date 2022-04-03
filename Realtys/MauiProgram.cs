using Microsoft.EntityFrameworkCore;
using Realtys.Database;
using Realtys.Models;
using Realtys.ViewModels;
using Realtys.Views;

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


		//builder.Services.AddSingleton<RealEstate>();
		//builder.Services.AddSingleton<Mortgage>();

		builder.Services.AddTransient<EditViewModel>();
		builder.Services.AddTransient<RE_EntryPage>();
		
		builder.Services.AddSingleton<RE_List>();


		var services = builder.Services.BuildServiceProvider();
		using (var scope = services.CreateScope())
		{

			var dbContext = scope.ServiceProvider.GetRequiredService<RealtysDbContext>();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();

			DatabaseInit dbInit = new();
			dbInit.Initialization(dbContext);


		}


		return builder.Build();
	}
}
