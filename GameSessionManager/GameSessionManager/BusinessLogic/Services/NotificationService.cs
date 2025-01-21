using Discord;
using Discord.WebSocket;
using NLog;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GameSessionManager.BusinessLogic.Services
{
    /// <summary>
    /// Основной сервис уведомлений, делегирующий отправку конкретным реализациям.
    /// </summary>
    public class NotificationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Инициализирует экземпляр NotificationService с выбранной реализацией уведомлений.
        /// </summary>
        /// <param name="notificationService">Реализация интерфейса INotificationService.</param>
        public NotificationService(INotificationService notificationService)
        {
            _notificationService = notificationService;
            Logger.Info("NotificationService инициализирован.");
        }

        /// <summary>
        /// Отправляет уведомление через выбранный сервис.
        /// </summary>
        /// <param name="message">Текст уведомления.</param>
        public async Task SendNotification(string message)
        {
            try
            {
                Logger.Info($"Отправка уведомления: {message}");
                await _notificationService.SendNotification(message);
                Logger.Info("Уведомление успешно отправлено.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при отправке уведомления.");
            }
        }
    }

    /// <summary>
    /// Интерфейс для сервисов уведомлений.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Отправляет уведомление.
        /// </summary>
        /// <param name="message">Текст уведомления.</param>
        Task SendNotification(string message);
    }

    /// <summary>
    /// Реализация сервиса уведомлений через Email.
    /// </summary>
    public class EmailNotificationService : INotificationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SmtpClient _emailSmtpClient;

        /// <summary>
        /// Инициализирует экземпляр EmailNotificationService.
        /// </summary>
        /// <param name="emailSmtpClient">Настроенный SMTP клиент.</param>
        public EmailNotificationService(SmtpClient emailSmtpClient)
        {
            _emailSmtpClient = emailSmtpClient;
            Logger.Info("EmailNotificationService инициализирован.");
        }

        /// <summary>
        /// Отправляет уведомление на email.
        /// </summary>
        /// <param name="message">Текст уведомления.</param>
        public async Task SendNotification(string message)
        {
            try
            {
                var mailMessage = new MailMessage("no-reply@example.com", "recipient@example.com", "Game Session Notification", message);
                Logger.Info("Попытка отправки email уведомления.");
                await _emailSmtpClient.SendMailAsync(mailMessage);
                Logger.Info("Email уведомление успешно отправлено.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при отправке email уведомления.");
            }
        }
    }

    /// <summary>
    /// Реализация сервиса уведомлений через Discord.
    /// </summary>
    public class DiscordNotificationService : INotificationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly ulong _channelId;

        /// <summary>
        /// Инициализирует экземпляр DiscordNotificationService.
        /// </summary>
        /// <param name="discordSocketClient">Клиент Discord для взаимодействия.</param>
        /// <param name="channelId">ID канала для отправки уведомлений.</param>
        public DiscordNotificationService(DiscordSocketClient discordSocketClient, ulong channelId)
        {
            _discordSocketClient = discordSocketClient;
            _channelId = channelId;
            Logger.Info("DiscordNotificationService инициализирован.");
        }

        /// <summary>
        /// Отправляет уведомление в указанный канал Discord.
        /// </summary>
        /// <param name="message">Текст уведомления.</param>
        public async Task SendNotification(string message)
        {
            try
            {
                Logger.Info("Попытка отправки уведомления в Discord.");
                var channel = _discordSocketClient.GetChannel(_channelId) as ITextChannel;
                if (channel != null)
                {
                    await channel.SendMessageAsync(message);
                    Logger.Info("Уведомление успешно отправлено в Discord.");
                }
                else
                {
                    Logger.Warn("Не удалось найти указанный канал Discord.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при отправке уведомления в Discord.");
            }
        }
    }
}
