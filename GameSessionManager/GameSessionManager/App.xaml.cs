using GameSessionManager.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using System;
using System.Net.Mail;
using System.Windows;
using NLog;

namespace GameSessionManager
{
    /// <summary>
    /// Основной класс приложения.
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Инициализирует приложение и запускает его.
        /// </summary>
        public App()
        {
            try
            {
                // Создаём и настраиваем сервисы.
                var serviceProvider = ConfigureServices();

                // Получаем главное окно и показываем его.
                var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
                Logger.Info("Приложение успешно запущено.");
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Ошибка при запуске приложения.");
                throw;
            }
        }

        /// <summary>
        /// Метод для конфигурации и регистрации всех необходимых сервисов.
        /// </summary>
        /// <returns>Провайдер сервисов.</returns>
        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            try
            {
                // Регистрация бизнес-логики.
                serviceCollection.AddSingleton<NotificationService>();
                serviceCollection.AddSingleton<ChallengeService>();
                serviceCollection.AddSingleton<VotingService>();

                // Регистрация Discord уведомлений.
                serviceCollection.AddSingleton<INotificationService>(provider =>
                    new DiscordNotificationService(new DiscordSocketClient(), 1234567890)); // ID канала

                // Регистрация Email уведомлений.
                serviceCollection.AddSingleton(new SmtpClient("smtp.example.com")); // SMTP сервер.
                serviceCollection.AddSingleton<EmailNotificationService>();

                // Регистрация главного окна.
                serviceCollection.AddSingleton<MainWindow>();

                Logger.Info("Сервисы успешно зарегистрированы.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при конфигурации сервисов.");
                throw;
            }

            // Построение и возвращение провайдера.
            return serviceCollection.BuildServiceProvider();
        }
    }
}
