using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TgBotApp.Data;
using TgBotApp.Helpers;
using TgBotApp.Models;

namespace TgBotApp.Services.Data
{
    public class BaseRepository : IBaseRepository
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public BaseRepository(DataContext context, ILogger<BaseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add(Reminder reminder)
        {
            try
            {
                _context.Add(reminder);
                _logger.LogInformation(LoggingEvents.AddEntryToDatabaseSuccessful, "Add {entry} to database successful", reminder.ToString());
            }
            catch (Exception e)
            {
                _logger.LogWarning(LoggingEvents.AddEntryToDatabaseFailed, e, "Add {entry} to database failed", reminder.ToString());
            }
        }

        public async Task DeleteByNthNumber(long chatId, int nthNumber)
        {
            var remindersFromChat = await GetReminders(chatId);
            var reminderForRemove = remindersFromChat.Skip(nthNumber - 1).FirstOrDefault();

            if (reminderForRemove == null)
            {
                _logger.LogWarning(LoggingEvents.DatabaseEntryNotFound, "Not found {number}-th entry from {chatId}", nthNumber, chatId);
                throw new ArgumentException("Not found {number}-th entry from {chatId}");
            }
            else
            {
                try
                {
                    _context.Remove(reminderForRemove);
                    _logger.LogInformation(LoggingEvents.RemoveEntryFromDatabaseSuccessful, "Remove {reminder} from {chatId} successful", reminderForRemove, chatId);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(LoggingEvents.DatabaseEntryDeleteFailed, e, "Remove {reminder} from {chatId} failed", reminderForRemove, chatId);
                }
            }
        }

        public async Task Delete(int id)
        {
            var reminder = await GetReminder(id);

            _context.Remove(reminder);
        }

        public async Task<IEnumerable<Reminder>> GetAllReminders()
        {
            var reminders = await _context.Reminders.ToListAsync();

            return reminders;
        }

        public async Task<IEnumerable<Reminder>> GetReminders(long chatId)
        {
            var reminders = await _context.Reminders.Where(r => r.ChatId == chatId).ToListAsync();

            return reminders;
        }

        public async Task<Reminder> GetReminder(int id)
        {
            var reminder = await _context.Reminders.FirstOrDefaultAsync(r => r.Id == id);

            return reminder;
        }

        public async Task<bool> SaveAll()
        {
            var changesSuccessful = await _context.SaveChangesAsync() > 0;

            if (changesSuccessful)
                _logger.LogInformation(LoggingEvents.AddChangesToDatabaseSuccessful, "Add changes to database successful!");
            else
                _logger.LogInformation(LoggingEvents.AddChangesToDatabaseNotFound, "Changes in database not found!");

            return changesSuccessful;
        }
    }
}
