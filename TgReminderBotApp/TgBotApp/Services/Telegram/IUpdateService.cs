using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBotApp.Services.Telegram
{
    public interface IUpdateService
    {
        Task Update(Update update);
    }
}
