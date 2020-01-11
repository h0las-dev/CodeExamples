namespace Social.Models
{
    using System;

    public struct Friend
    {
        public int FromUserId;

        public DateTime SendDate;

        public int Status;

        public int ToUserId;

        public Friend(int fromId, DateTime sendDate, int status, int toId)
        {
            this.FromUserId = fromId;
            this.SendDate = sendDate;
            this.Status = status;
            this.ToUserId = toId;
        }

        public override bool Equals(object obj)
        {
            var friend = (Friend)obj;

            if (friend.FromUserId == this.FromUserId && friend.SendDate == this.SendDate && friend.Status == this.Status && friend.ToUserId == this.ToUserId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.FromUserId.GetHashCode() ^ this.SendDate.GetHashCode() ^ this.Status.GetHashCode() ^ this.ToUserId.GetHashCode();
        }
    }
}