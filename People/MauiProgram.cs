using Microsoft.Extensions.Logging;
using People.Helpers;
using People.Services;

namespace People;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Регистрируем репозиторий с путём к базе данных
        string dbPath = FileAccessHelper.GetLocalFilePath("people.db3");
        builder.Services.AddSingleton(new PersonRepository(dbPath));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}