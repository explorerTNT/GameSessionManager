using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace GameSessionManager.BusinessLogic.Services
{
    public class VotingService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly NotificationService _notificationService;

        /// <summary>
        /// Хранит список голосующих и их выбор.
        /// </summary>
        private readonly Dictionary<string, string> _votes;

        /// <summary>
        /// Инициализирует экземпляр класса VotingService.
        /// </summary>
        /// <param name="notificationService">Сервис для отправки уведомлений.</param>
        public VotingService(NotificationService notificationService)
        {
            _notificationService = notificationService;
            _votes = new Dictionary<string, string>();
            Logger.Info("VotingService инициализирован.");
        }

        /// <summary>
        /// Запускает процесс голосования и отправляет уведомление об этом.
        /// </summary>
        public void StartVoting()
        {
            Logger.Info("Голосование начато.");
            Console.WriteLine("Голосование начато!");

            try
            {
                _notificationService.SendNotification("Голосование началось! Проголосуйте за вариант A или B.");
                Logger.Info("Уведомление о начале голосования отправлено.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при отправке уведомления о начале голосования.");
            }
        }

        /// <summary>
        /// Регистрирует голос пользователя.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="voteChoice">Выбор пользователя.</param>
        public void Vote(string username, string voteChoice)
        {
            try
            {
                if (!_votes.ContainsKey(username))
                {
                    _votes.Add(username, voteChoice);
                    Logger.Info($"Пользователь {username} проголосовал за {voteChoice}.");
                }
                else
                {
                    _votes[username] = voteChoice;
                    Logger.Info($"Пользователь {username} изменил голос на {voteChoice}.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при обработке голоса пользователя {username}.");
            }
        }

        /// <summary>
        /// Получает результаты голосования.
        /// </summary>
        /// <returns>Строка с результатами голосования.</returns>
        public string GetVoteResult()
        {
            int voteA = 0;
            int voteB = 0;

            foreach (var vote in _votes.Values)
            {
                if (vote == "A")
                    voteA++;
                else if (vote == "B")
                    voteB++;
            }

            Logger.Info($"Результаты голосования получены: A - {voteA}, B - {voteB}.");
            return $"Голосование завершено. Результаты: A - {voteA} голос(ов), B - {voteB} голос(ов).";
        }

        /// <summary>
        /// Обрабатывает результаты голосования и отправляет уведомление.
        /// </summary>
        public async Task ProcessVote()
        {
            try
            {
                string voteResult = GetVoteResult();
                await _notificationService.SendNotification(voteResult);
                Logger.Info("Уведомление с результатами голосования отправлено.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при обработке голосования.");
            }
        }

        /// <summary>
        /// Сбрасывает результаты голосования.
        /// </summary>
        public void ResetVotes()
        {
            _votes.Clear();
            Logger.Info("Результаты голосования сброшены.");
        }
    }
}
