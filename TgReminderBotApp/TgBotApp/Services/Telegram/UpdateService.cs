using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotApp.Helpers;
using TgBotApp.Services.Data;
using TgBotApp.Services.Telegram.Handlers;

namespace TgBotApp.Services.Telegram
{
    public class UpdateService : IUpdateService
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public UpdateService(ILogger<UpdateService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Update(Update update)
        {
            var message = update.Message;

            if (message.Type != MessageType.Text)
                return;

            try
            {
                await CheckMessage(message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong. {e}");
                throw;
            }
        }

        private async Task CheckMessage(Message message)
        {
            if (message.Text.StartsWith(BotCommands.Start))
            {
                await _mediator.Send(new StartCommand(message));
            }
            else if (message.Text.StartsWith(BotCommands.Add))
            {
                await _mediator.Send(new AddCommand(message));
            }
            else if (message.Text.StartsWith(BotCommands.List))
            {
                await _mediator.Send(new ListCommand(message));
            }
            else if (message.Text.StartsWith(BotCommands.Delete))
            {
                await _mediator.Send(new DeleteCommand(message));
            }
            else
            {
                _logger.LogError(LoggingEvents.RecognizeCommandFailed, $"Can't recognize user command. The message was: {message.Text}");
                await _mediator.Send(new ErrorCommand(message));
            }
        }
    }
}
