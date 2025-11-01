using Microsoft.Extensions.Logging;
using SignalRClient.Interfaces;
using SignalRClient.Services;
using SignalRClient.ViewModels;
using SignalRClient.Views;

namespace SignalRClient
{
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<ISignalRService, SignalRService>();


            builder.Services.AddSingleton<ChatViewModel>();
            builder.Services.AddTransient<ChatView>();

            return builder.Build();
        }
    }
}
