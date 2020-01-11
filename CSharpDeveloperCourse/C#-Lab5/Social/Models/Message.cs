namespace Social.Models
{
    using System;
    using System.Collections.Generic;

    public struct Message
    {
        public int AuthorId;

        public List<int> Likes;

        public int MessageId;

        public DateTime SendDate;

        public string Text;

        public Message(int id, List<int> likes, int msgId, DateTime sendDate, string text)
        {
            this.AuthorId = id;
            this.Likes = likes;
            this.MessageId = msgId;
            this.SendDate = sendDate;
            this.Text = text;
        }
    }
}