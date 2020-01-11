using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotApp.Helpers;
using TgBotApp.Services.Data;

namespace TgBotApp.Services.Telegram.Handlers
{
    public class DeleteCommand : IRequest
    {
        public Message Message;

        public DeleteCommand(Message message)
        {
            Message = message;
        }
    }
    
    public class DeleteCommandHandler : AsyncRequestHandler<DeleteCommand>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IBaseRepository _repository;
        private readonly ILogger _logger;

        public DeleteCommandHandler(ITelegramBotService telegramBotService, IBaseRepository repository, ILogger<DeleteCommandHandler> logger)
        {
            _telegramBotService = telegramBotService;
            _repository = repository;
            _logger = logger;
        }

        protected override async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var chatId = request.Message.Chat.Id;
            var client = _telegramBotService.GetClient();

            try
            {
                var nthNumber = GetReminderNthNumberFromMessage(request.Message);
                await _repository.DeleteByNthNumber(chatId, nthNumber);
                await _repository.SaveAll();

                var chatBotResponse = string.Format(StringConstants.SuccessfulDeleteReminder, nthNumber);
                await client.SendTextMessageAsync(chatId, chatBotResponse, replyToMessageId: request.Message.MessageId,
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.DatabaseEntryDeleteFailed, e, "Can't remove entry from db. Check user command: {0}", request.Message.Text);
                await client.SendTextMessageAsync(chatId, StringConstants.FailedDeleteReminder, replyToMessageId: request.Message.MessageId, cancellationToken: cancellationToken);
            }
        }

        private int GetReminderNthNumberFromMessage(Message message)
        {
            var nthNumber = Convert.ToInt32(message.Text.Replace("/del", string.Empty).
                Replace(@"\s+", string.Empty));

            return nthNumber;
        }
    }
}
