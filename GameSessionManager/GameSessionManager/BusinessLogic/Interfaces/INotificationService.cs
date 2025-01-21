using System.Threading.Tasks;

namespace GameSessionManager.BusinessLogic.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с уведомлениями.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Отправляет email-уведомление.
        /// </summary>
        /// <param name="to">Адрес получателя.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Текст письма.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task SendEmailNotification(string to, string subject, string body);

        /// <summary>
        /// Отправляет уведомление в Discord.
        /// </summary>
        /// <param name="message">Сообщение для отправки.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task SendDiscordNotification(string message);

        /// <summary>
        /// Отправляет массовое уведомление по email нескольким адресатам.
        /// </summary>
        /// <param name="recipients">Список email-адресов получателей.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Текст письма.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task SendBulkEmailNotification(string[] recipients, string subject, string body);

        /// <summary>
        /// Отправляет уведомление в несколько каналов Discord.
        /// </summary>
        /// <param name="channelIds">Список идентификаторов каналов Discord.</param>
        /// <param name="message">Сообщение для отправки.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task SendDiscordNotificationToChannels(ulong[] channelIds, string message);
    }
}
