using GameSessionManager.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using NLog;

namespace GameSessionManager
{
    /// <summary>
    /// Главный класс окна приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotificationService _notificationService;
        private readonly VotingService _votingService;
        private readonly ChallengeService _challengeService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Конструктор с внедрением зависимостей.
        /// </summary>
        /// <param name="notificationService">Сервис уведомлений.</param>
        /// <param name="votingService">Сервис голосований.</param>
        /// <param name="challengeService">Сервис управления челенджами.</param>
        public MainWindow(NotificationService notificationService, VotingService votingService, ChallengeService challengeService)
        {
            InitializeComponent();
            _notificationService = notificationService;
            _votingService = votingService;
            _challengeService = challengeService;

            Logger.Info("Главное окно успешно инициализировано.");
        }

        /// <summary>
        /// Обработчик кнопки для отправки уведомления.
        /// </summary>
        private async void SendNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            string message = "This is a test notification from MainWindow!";
            try
            {
                await _notificationService.SendNotification(message);
                MessageBox.Show("Уведомление успешно отправлено.");
                Logger.Info("Уведомление отправлено: {0}", message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при отправке уведомления.");
                MessageBox.Show($"Ошибка при отправке уведомления: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик кнопки для начала голосования.
        /// </summary>
        private void StartVotingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _votingService.StartVoting();
                MessageBox.Show("Голосование успешно начато.");
                Logger.Info("Голосование начато.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при запуске голосования.");
                MessageBox.Show($"Ошибка при запуске голосования: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик кнопки для начала челенджа.
        /// </summary>
        //private async void StartChallengeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string challengeName = ChallengeNameTextBox.Text?.Trim();

        //    if (!string.IsNullOrEmpty(challengeName))
        //    {
        //        try
        //        {
        //            await _challengeService.StartChallenge(challengeName);
        //            MessageBox.Show($"Челендж '{challengeName}' успешно начат.");
        //            Logger.Info("Челендж '{0}' начат.", challengeName);
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Error(ex, "Ошибка при запуске челенджа '{0}'.", challengeName);
        //            MessageBox.Show($"Ошибка: {ex.Message}");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Пожалуйста, введите название челенджа.");
        //        Logger.Warn("Попытка начать челендж без указания названия.");
        //    }
        //}
    }
}
