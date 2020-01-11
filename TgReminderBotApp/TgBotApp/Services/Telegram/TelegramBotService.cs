using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using TgBotApp.Helpers;

namespace TgBotApp.Services.Telegram
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly ILogger _logger;
        private TelegramBotClient _client;

        public TelegramBotService(ILogger<TelegramBotService> logger)
        {
            _logger = logger;

            _client = new TelegramBotClient(TelegramConfig.Token);
        }

        public TelegramBotClient GetClient()
        {
            return _client;
        }

        public async Task SetWebhookAsync()
        {
            _client = new TelegramBotClient(TelegramConfig.Token);
            var hook = string.Format(TelegramConfig.Url, "api/messages");

            _logger.LogInformation(LoggingEvents.ConnectToTelegram, "Setting a webhook via url: {url}", hook);

            try
            {
                await _client.SetWebhookAsync(hook);
                _logger.LogInformation(LoggingEvents.ConnectToTelegramSuccessful, "Webhook setup was successful via url: {url}", hook);
            }
            catch (Exception e)
            {
                _logger.LogWarning(LoggingEvents.ConnectToTelegramFailed, e, "Webhook setup failed via url: {url}", hook);
            }
        }

        public async Task DeleteWebhookAsync()
        {
            try
            {
                await _client.DeleteWebhookAsync();
                _logger.LogInformation(LoggingEvents.ConnectToTelegramSuccessful, "Webhook delete was successful");
            }
            catch (Exception e)
            {
                _logger.LogWarning(LoggingEvents.ConnectToTelegramFailed, e, "Webhook delete failed");
            }
        }
    }
}
