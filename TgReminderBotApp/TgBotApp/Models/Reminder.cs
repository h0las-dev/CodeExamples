using System;

namespace TgBotApp.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public long ChatId { get; set; }
        public DateTime Date { get; set; }
    }
}
