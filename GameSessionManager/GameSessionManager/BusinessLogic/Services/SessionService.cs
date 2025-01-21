//using GameSessionManager.DataLayer.Models;
//using NLog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GameSessionManager.BusinessLogic.Services
//{
//    /// <summary>
//    /// Сервис для управления игровыми сессиями.
//    /// </summary>
//    public class SessionService
//    {
//        private readonly List<Session> _sessions;
//        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

//        /// <summary>
//        /// Конструктор сервиса.
//        /// </summary>
//        public SessionService()
//        {
//            _sessions = new List<Session>();
//            Logger.Info("Сервис SessionService успешно инициализирован.");
//        }

//        /// <summary>
//        /// Создаёт новую игровую сессию.
//        /// </summary>
//        /// <param name="scheduledTime">Время запланированной сессии.</param>
//        /// <param name="participants">Список участников.</param>
//        /// <param name="game">Игра, связанная с сессией.</param>
//        public async Task CreateSession(DateTime scheduledTime, List<User> participants, Game game)
//        {
//            if (participants == null || participants.Count == 0)
//            {
//                Logger.Warn("Попытка создать сессию без участников.");
//                throw new ArgumentException("Сессия должна содержать хотя бы одного участника.");
//            }

//            if (game == null)
//            {
//                Logger.Warn("Попытка создать сессию без указания игры.");
//                throw new ArgumentException("Игра должна быть указана.");
//            }

//            try
//            {
//                var session = new Session
//                {
//                    Id = _sessions.Count > 0 ? _sessions.Max(s => s.Id) + 1 : 1, // Генерация уникального ID
//                    ScheduledTime = scheduledTime,
//                    Participants = participants,
//                    Game = game,
//                    Status = "Planned"
//                };

//                _sessions.Add(session);
//                Logger.Info("Создана новая сессия с ID {0} для игры {1} на {2}.", session.Id, game.Name, scheduledTime);

//                // Имитация отправки уведомлений участникам
//                foreach (var participant in participants)
//                {
//                    var message = $"You have been added to a new game session for {game.Name} scheduled at {scheduledTime}.";
//                    Logger.Info("Отправлено уведомление участнику {0}: {1}", participant.Name, message);
//                    // await _notificationService.SendNotification(message); // Интеграция с NotificationService
//                }

//                await Task.CompletedTask; // Асинхронное завершение метода
//            }
//            catch (Exception ex)
//            {
//                Logger.Error(ex, "Ошибка при создании сессии.");
//                throw;
//            }
//        }

//        /// <summary>
//        /// Возвращает список всех игровых сессий.
//        /// </summary>
//        public async Task<List<Session>> GetAllSessions()
//        {
//            try
//            {
//                Logger.Info("Получен список всех сессий (количество: {0}).", _sessions.Count);
//                return await Task.FromResult(_sessions);
//            }
//            catch (Exception ex)
//            {
//                Logger.Error(ex, "Ошибка при получении списка сессий.");
//                throw;
//            }
//        }

//        /// <summary>
//        /// Возвращает игровую сессию по её ID.
//        /// </summary>
//        /// <param name="sessionId">ID сессии.</param>
//        public async Task<Session> GetSessionById(int sessionId)
//        {
//            try
//            {
//                var session = _sessions.FirstOrDefault(s => s.Id == sessionId);

//                if (session == null)
//                {
//                    Logger.Warn("Сессия с ID {0} не найдена.", sessionId);
//                    throw new KeyNotFoundException($"Сессия с ID {sessionId} не найдена.");
//                }

//                Logger.Info("Найдена сессия с ID {0}.", sessionId);
//                return await Task.FromResult(session);
//            }
//            catch (Exception ex)
//            {
//                Logger.Error(ex, "Ошибка при получении сессии с ID {0}.", sessionId);
//                throw;
//            }
//        }
//    }
//}
