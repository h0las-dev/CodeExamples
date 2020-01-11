using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBotApp
{
    public static class TelegramConfig
    {
        public static string Token { get; set; } = @"<your_token>";
        public static string Url { get; set; } = @"<your_url>/{0}"; 
    }
}
