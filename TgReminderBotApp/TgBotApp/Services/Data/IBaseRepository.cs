using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TgBotApp.Models;

namespace TgBotApp.Services.Data
{
    public interface IBaseRepository
    {
        void Add(Reminder reminder);
        Task DeleteByNthNumber(long chatId, int nthNumber);
        Task Delete(int id);
        Task<IEnumerable<Reminder>> GetAllReminders();
        Task<IEnumerable<Reminder>> GetReminders(long chatId);
        Task<Reminder> GetReminder(int id);
        Task<bool> SaveAll();
    }
}
