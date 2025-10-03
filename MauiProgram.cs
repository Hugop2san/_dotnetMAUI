using Microsoft.Extensions.Logging;
using kogui.Services; // para chamar minha classe
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;




namespace kogui
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

            //adicionando meu serviço
            builder.Services.AddHttpClient<CorService>(); //REQUISITO 2
            //Teste adicionando a main page
            builder.Services.AddSingleton<MainPage>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }




    }
}
