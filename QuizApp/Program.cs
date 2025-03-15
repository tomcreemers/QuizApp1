using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.Repositories;
using QuizApp.Models;

namespace QuizApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var app = CreateMauiApp();
            app.Run();
        }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Repositories registreren in DI-container
            builder.Services.AddSingleton<GenericRepository<Participant>>();
            builder.Services.AddSingleton<GenericRepository<Session>>();
            builder.Services.AddSingleton<GenericRepository<QuizQuestion>>();
            builder.Services.AddSingleton<GenericRepository<Topic>>();

            return builder.Build();
        }
    }
}
