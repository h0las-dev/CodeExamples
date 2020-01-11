namespace Social.Models
{
    public class UserInformation
    {
        private string userName;

        private bool online;

        private int userId;

        public UserInformation(string name, bool status, int id)
        {
            this.UserName = name;
            this.Online = status;
            this.UserId = id;
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                this.userName = value;
            }
        }

        public bool Online
        {
            get
            {
                return this.online;
            }

            set
            {
                this.online = value;
            }
        }

        public int UserId
        {
            get
            {
                return this.userId;
            }

            set
            {
                this.userId = value;
            }
        }

        public override bool Equals(object obj)
        {
            var user = (UserInformation)obj;

            if (user.Online == this.Online && user.UserName == this.UserName && user.UserId == this.UserId)
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
            return this.UserName.GetHashCode() ^ this.Online.GetHashCode() ^ this.UserId.GetHashCode();
        }
    }
}