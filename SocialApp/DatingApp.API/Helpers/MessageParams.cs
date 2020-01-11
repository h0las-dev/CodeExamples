using System;

namespace DatingApp.API.Helpers
{
    public class MessageParams
    {
        private const int MaxPageSize = 50;
        private int pageSize = 10;
        
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize: value; }
        }

        public int UserId { get; set; }
        public string Container { get; set; } = MessageContainer.Unread.MessageContainerToString();
    }

    public enum MessageContainer
    {
        Unread,
        Inbox,
        Outbox
    }

    public static class MessageContainerStrings
    {
        public const string Unread = "Unread";
        public const string Inbox = "Inbox";
        public const string Outbox = "Outbox";

        public static MessageContainer MapStringToContainer(string value)
        {
            switch(value)
            {
                case Unread:
                    return MessageContainer.Unread;
                case Inbox:
                    return MessageContainer.Inbox;
                case Outbox:
                    return MessageContainer.Outbox;
                default:
                    throw new Exception("Can not convert string to MessageContainer");
            }
        }
    }
}