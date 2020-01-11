using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TgBotApp.Services.Telegram
{
    public interface ITelegramBotService
    {
        TelegramBotClient GetClient();
        Task SetWebhookAsync();
        Task DeleteWebhookAsync();
    }
}
