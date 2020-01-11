using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotApp.Helpers;
using TgBotApp.Models;
using TgBotApp.Services.Data;

namespace TgBotApp.Services.Telegram.Handlers
{
    public class ListCommand : IRequest
    {
        public Message Message;

        public ListCommand(Message message)
        {
            Message = message;
        }
    }

    public class ListCommandHandler : AsyncRequestHandler<ListCommand>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IBaseRepository _repository;

        public ListCommandHandler(ITelegramBotService telegramBotService, IBaseRepository repository)
        {
            _telegramBotService = telegramBotService;
            _repository = repository;
        }

        protected override async Task Handle(ListCommand request, CancellationToken cancellationToken)
        {
            var chatId = request.Message.Chat.Id;
            var client = _telegramBotService.GetClient();

            var reminders = await _repository.GetReminders(chatId);
            var remindersList = reminders.ToList();

            if (remindersList.Count() != 0)
            {
                var result = MapRemindersToString(remindersList);
                await client.SendTextMessageAsync(chatId, result, replyToMessageId: request.Message.MessageId,
                    parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
            }
            else
            {
                await client.SendTextMessageAsync(chatId, StringConstants.RemindersNotFound, replyToMessageId: request.Message.MessageId,
                    parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
            }
        }

        private string MapRemindersToString(IEnumerable<Reminder> reminders)
        {
            var reminderString = new StringBuilder(StringConstants.ReminderListHeader);
            var counter = 0;

            foreach (var reminder in reminders)
            {
                counter++;
                reminderString.Append(string.Format(StringConstants.ReminderStringView, counter, reminder.Date,
                    reminder.Text));
            }

            return reminderString.ToString();
        }
    }
}
