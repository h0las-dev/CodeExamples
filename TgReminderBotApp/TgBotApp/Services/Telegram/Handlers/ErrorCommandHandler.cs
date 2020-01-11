using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotApp.Helpers;

namespace TgBotApp.Services.Telegram.Handlers
{
    public class ErrorCommand : IRequest
    {
        public Message Message;

        public ErrorCommand(Message message)
        {
            Message = message;
        }
    }

    public class ErrorCommandHandler : AsyncRequestHandler<ErrorCommand>
    {
        private readonly ITelegramBotService _telegramBotService;

        public ErrorCommandHandler(ITelegramBotService telegramBotService)
        {
            _telegramBotService = telegramBotService;
        }

        protected override async Task Handle(ErrorCommand request, CancellationToken cancellationToken)
        {
            var chatId = request.Message.Chat.Id;
            var client = _telegramBotService.GetClient();
            await client.SendTextMessageAsync(chatId, StringConstants.ErrorMessage, replyToMessageId: request.Message.MessageId, cancellationToken: cancellationToken);
        }
    }
}
