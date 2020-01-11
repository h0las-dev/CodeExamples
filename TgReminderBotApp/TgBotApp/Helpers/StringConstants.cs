using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBotApp.Helpers
{
    public static class StringConstants
    {
        public static string StartMessage =
            "Привет! Я - маленький бот, написанный на платформе asp .net core 3.0. " +
            "Я умею хранить ваши напоминания, распознавать даты и текст в повседневной речи " +
            "и напоминать вам о важных задачах в назначенное время :)\nСписок моих комманд:";

        public static string ErrorMessage =
            "Не могу распознать команду :( Попробуйте снова.";

        public static string ErrorRecognizeDate =
            "Не могу распознать дату в вашем сообщении :( Уточните дату и попробуйте снова.";

        public static string AddReminderSuccessful =
            "Напомню *{0}* о том, что вы планируете: *{1}*";

        public static string ReminderStringView = 
            "{0}) *{1}* - {2}\n";

        public static string ReminderListHeader =
            "Текущие напоминания:\n\n";

        public static string RemindersNotFound =
            "У вас нет текущих напоминаний";

        public static string SuccessfulDeleteReminder =
            "Напоминание №{0} было успешно удалено!";

        public static string ReminderHelperText =
            "Напоминаю, что *{0}* вы планируете: *{1}*";

        public static string SorryReminderHelperText =
            "Забыл напомнить вам, что в *{0}* вы планировали: *{1}*.\nПростите, боты тоже всё забывают ;c";

        public static string FailedDeleteReminder =
            "Не могу удалить ваше напоминание. Проверьте номер и попробуйте снова.";

        public static string CouldNotRecognizeCommand = 
            "Не могу распознать комманду, проверьте правильность ввода!";

    }
}
