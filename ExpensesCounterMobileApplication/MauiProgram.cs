
using Microsoft.Extensions.Logging;

namespace ExpensesCounterMobileApplication
{
    public static class MauiProgram // Main configuration point, which was created automaticly
    {
        public static MauiApp CreateMauiApp() // Constaructor of the hall aplication
        {
            var builder = MauiApp.CreateBuilder(); // Create builder, which constarct the hall aplication
            builder
                .UseMauiApp<App>() // Definee the first page of the application and the overall lifecycle of the application.
                .ConfigureFonts(fonts => // Set fonts
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); // Add font
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); // Add font
                });

#if DEBUG
            builder.Logging.AddDebug(); // Debug
#endif

            return builder.Build(); // Return final object of MauiApp
        }
    }
}
