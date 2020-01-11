using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBotApp.Helpers
{
    public static class LoggingEvents
    {
        public const int ConnectToTelegram = 1000;
        public const int RunStartCommand = 1001;
        public const int RunListCommand = 1002;
        public const int EmptyReminderList = 1003;

        public const int ConnectToTelegramSuccessful = 2000;
        public const int RunStartCommandSuccessful = 2001;
        public const int RunListCommandSuccessful = 2002;
        public const int AddEntryToDatabaseSuccessful = 2003;
        public const int RemoveEntryFromDatabaseSuccessful = 2004;
        public const int AddChangesToDatabaseSuccessful = 2005;

        public const int ReminderExecutorJobComplete = 2100;

        public const int ConnectToTelegramFailed = 4000;
        public const int RecognizeCommandFailed = 4001;
        public const int RunStartCommandFailed = 4002;
        public const int RunListCommandFailed = 4003;
        public const int AddEntryToDatabaseFailed = 4004;
        public const int DatabaseEntryNotFound = 4005;
        public const int DatabaseEntryDeleteFailed = 4006;
        public const int AddChangesToDatabaseNotFound = 4007;
        public const int RecognizeDateFailed = 4008;
    }
}
