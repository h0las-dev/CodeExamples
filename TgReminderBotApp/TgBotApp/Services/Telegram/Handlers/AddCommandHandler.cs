using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hors;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotApp.Helpers;
using TgBotApp.Models;
using TgBotApp.Services.Data;

namespace TgBotApp.Services.Telegram.Handlers
{
    public class AddCommand : IRequest
    {
        public Message Message;

        public AddCommand(Message message)
        {
            Message = message;
        }
    }

    public class AddCommandHandler : AsyncRequestHandler<AddCommand>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IBaseRepository _repository;
        private readonly ILogger _logger;

        public AddCommandHandler(ITelegramBotService telegramBotService, IBaseRepository repository, ILogger<AddCommandHandler> logger)
        {
            _telegramBotService = telegramBotService;
            _repository = repository;
            _logger = logger;
        }

        protected override async Task Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var chatId = request.Message.Chat.Id;
            var client = _telegramBotService.GetClient();
            Reminder reminder;

            try
            {
                reminder = MapMessageToReminder(request.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.RecognizeDateFailed, e, "Can't recognize date in user message. Message: {0}", request.Message.Text);
                await client.SendTextMessageAsync(chatId, StringConstants.ErrorRecognizeDate, replyToMessageId: request.Message.MessageId, cancellationToken: cancellationToken);
                return;
            }
            
            _repository.Add(reminder);
            await _repository.SaveAll();

            var chatBotResponse = string.Format(StringConstants.AddReminderSuccessful, reminder.Date, reminder.Text);
            await client.SendTextMessageAsync(chatId, chatBotResponse, replyToMessageId: request.Message.MessageId, 
                parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
        }

        private Reminder MapMessageToReminder(Message message)
        {
            var chatId = message.Chat.Id;
            var userInput = message.Text.Replace(BotCommands.Add, string.Empty);

            var horsParser = new HorsTextParser();
            var parseResult = horsParser.Parse(userInput, DateTime.Now);

            var reminder = new Reminder
            {
                Text = parseResult.Text,
                Date = parseResult.Dates[0].DateFrom,
                ChatId = chatId
            };

            return reminder;
        }
    }
}
