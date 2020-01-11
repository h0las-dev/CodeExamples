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
    public class StartCommand : IRequest
    {
        public Message Message;

        public StartCommand(Message message)
        {
            Message = message;
        }
    }

    public class StartCommandHandler : AsyncRequestHandler<StartCommand>
    {
        private readonly ITelegramBotService _telegramBotService;

        public StartCommandHandler(ITelegramBotService telegramBotService)
        {
            _telegramBotService = telegramBotService;
        }

        protected override async Task Handle(StartCommand request, CancellationToken cancellationToken)
        {
            var chatId = request.Message.Chat.Id;
            var client = _telegramBotService.GetClient();
            await client.SendTextMessageAsync(chatId, StringConstants.StartMessage, ParseMode.Markdown, 
                cancellationToken: cancellationToken);
        }
    }
}
