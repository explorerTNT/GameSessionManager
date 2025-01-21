using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace GameSessionManager.BusinessLogic.Services
{
    /// <summary>
    /// Сервис для управления игровыми челенджами.
    /// </summary>
    public class ChallengeService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly NotificationService _notificationService;

        /// <summary>
        /// Список активных челенджей.
        /// </summary>
        private readonly List<Challenge> _activeChallenges;

        /// <summary>
        /// Инициализирует экземпляр ChallengeService.
        /// </summary>
        /// <param name="notificationService">Сервис уведомлений.</param>
        public ChallengeService(NotificationService notificationService)
        {
            _notificationService = notificationService;
            _activeChallenges = new List<Challenge>();
            Logger.Info("ChallengeService инициализирован.");
        }

        /// <summary>
        /// Создаёт новый челендж.
        /// </summary>
        /// <param name="challengeName">Название челенджа.</param>
        /// <param name="description">Описание челенджа.</param>
        /// <param name="startDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        public void CreateChallenge(string challengeName, string description, DateTime startDate, DateTime endDate)
        {
            var newChallenge = new Challenge
            {
                Name = challengeName,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                Status = ChallengeStatus.Pending
            };

            _activeChallenges.Add(newChallenge);
            Logger.Info($"Челендж '{challengeName}' создан.");
        }

        /// <summary>
        /// Начинает челендж с указанным именем.
        /// </summary>
        /// <param name="challengeName">Название челенджа.</param>
        public async Task StartChallenge(string challengeName)
        {
            var challenge = _activeChallenges.FirstOrDefault(c => c.Name == challengeName);
            if (challenge != null && challenge.Status == ChallengeStatus.Pending)
            {
                challenge.Status = ChallengeStatus.Ongoing;

                Logger.Info($"Челендж '{challengeName}' начат.");
                await _notificationService.SendNotification($"Челендж '{challengeName}' начался!");
            }
            else
            {
                Logger.Warn($"Попытка начать челендж '{challengeName}' не удалась. Причина: не найден или уже начат.");
                throw new InvalidOperationException("Челендж не найден или уже начат.");
            }
        }

        /// <summary>
        /// Завершает челендж с указанным именем.
        /// </summary>
        /// <param name="challengeName">Название челенджа.</param>
        public async Task CompleteChallenge(string challengeName)
        {
            var challenge = _activeChallenges.FirstOrDefault(c => c.Name == challengeName);
            if (challenge != null && challenge.Status == ChallengeStatus.Ongoing)
            {
                challenge.Status = ChallengeStatus.Completed;

                Logger.Info($"Челендж '{challengeName}' завершён.");
                await _notificationService.SendNotification($"Челендж '{challengeName}' завершён!");

                // Дополнительная логика обработки результатов.
                await ProcessChallengeResults(challenge);
            }
            else
            {
                Logger.Warn($"Попытка завершить челендж '{challengeName}' не удалась. Причина: не найден или не в процессе.");
                throw new InvalidOperationException("Челендж не найден или не в процессе.");
            }
        }

        /// <summary>
        /// Отменяет челендж с указанным именем.
        /// </summary>
        /// <param name="challengeName">Название челенджа.</param>
        public void CancelChallenge(string challengeName)
        {
            var challenge = _activeChallenges.FirstOrDefault(c => c.Name == challengeName);
            if (challenge != null && challenge.Status == ChallengeStatus.Pending)
            {
                _activeChallenges.Remove(challenge);

                Logger.Info($"Челендж '{challengeName}' отменён.");
                _notificationService.SendNotification($"Челендж '{challengeName}' был отменён.");
            }
            else
            {
                Logger.Warn($"Попытка отменить челендж '{challengeName}' не удалась. Причина: не найден или уже начат.");
                throw new InvalidOperationException("Челендж не найден или уже начат.");
            }
        }

        /// <summary>
        /// Возвращает список активных челенджей.
        /// </summary>
        /// <returns>Список активных челенджей.</returns>
        public List<Challenge> GetActiveChallenges()
        {
            Logger.Info("Получен список активных челенджей.");
            return _activeChallenges.Where(c => c.Status == ChallengeStatus.Ongoing).ToList();
        }

        /// <summary>
        /// Обрабатывает результаты завершённого челенджа.
        /// </summary>
        /// <param name="challenge">Завершённый челендж.</param>
        private async Task ProcessChallengeResults(Challenge challenge)
        {
            Logger.Info($"Обработка результатов челенджа '{challenge.Name}'.");
            // Здесь будет логика для подсчёта очков, определения победителей и т. д.
            await _notificationService.SendNotification($"Челендж '{challenge.Name}' завершён. Подробности будут позже.");
        }
    }

    /// <summary>
    /// Перечисление статусов челенджа.
    /// </summary>
    public enum ChallengeStatus
    {
        Pending,    // Челендж ожидает начала.
        Ongoing,    // Челендж в процессе.
        Completed   // Челендж завершён.
    }

    /// <summary>
    /// Класс, представляющий челендж.
    /// </summary>
    public class Challenge
    {
        /// <summary>
        /// Название челенджа.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание челенджа.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата начала челенджа.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания челенджа.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Статус челенджа.
        /// </summary>
        public ChallengeStatus Status { get; set; }
    }
}
