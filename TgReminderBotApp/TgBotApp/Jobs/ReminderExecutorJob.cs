using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using TgBotApp.Helpers;
using TgBotApp.Models;
using TgBotApp.Services.Data;
using TgBotApp.Services.Telegram;

namespace TgBotApp.Jobs
{
    public class ReminderExecutorJob : IReminderExecutorJob
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IBaseRepository _repository;
        private readonly ILogger _logger;

        public ReminderExecutorJob(ITelegramBotService telegramBotService, IBaseRepository repository, ILogger<ReminderExecutorJob> logger)
        {
            _telegramBotService = telegramBotService;
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteReminders()
        {
            _logger.LogWarning(LoggingEvents.ReminderExecutorJobComplete, message: "ReminderExecutorJob is starting now.");

            var client = _telegramBotService.GetClient();
            var reminders = await _repository.GetAllReminders();
            var comingReminders = GetComingReminders(reminders);

            foreach (var reminder in comingReminders)
            {
                var clientResponse = string.Format(
                    DateTime.Now < reminder.Date ? 
                        StringConstants.ReminderHelperText : 
                        StringConstants.SorryReminderHelperText, 
                    reminder.Date, 
                    reminder.Text);

                await client.SendTextMessageAsync(reminder.ChatId, clientResponse, ParseMode.Markdown);

                await _repository.Delete(reminder.Id);

                await _repository.SaveAll();
            }

            _logger.LogWarning(LoggingEvents.ReminderExecutorJobComplete, message: "ReminderExecutorJob is ending now.");
        }

        private IEnumerable<Reminder> GetComingReminders(IEnumerable<Reminder> reminders)
        {
            const int dateOffset = 300; // in seconds, i.e. 5 minutes

            var remindersForReturn = new List<Reminder>();

            foreach (var reminder in reminders)
            {
                var dateDifference = reminder.Date.Subtract(DateTime.Now).TotalSeconds;
                if (dateDifference <= dateOffset || DateTime.Now > reminder.Date)
                {
                    remindersForReturn.Add(reminder);
                }
            }

            return remindersForReturn;
        }
    }
}
