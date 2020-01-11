namespace Social
{
    using System.Collections.Generic;
    using Models;

    public class UserContext
    {
        private User user;

        private List<UserInformation> friends;

        private List<UserInformation> onlineFriends;

        private List<UserInformation> friendshipOffers;

        private List<UserInformation> subscribers;

        private List<News> news;

        public User User
        {
            get
            {
                return this.user;
            }

            set
            {
                this.user = value;
            }
        }

        public List<UserInformation> Friends
        {
            get
            {
                return this.friends;
            }

            set
            {
                this.friends = value;
            }
        }

        public List<UserInformation> OnlineFriends
        {
            get
            {
                return this.onlineFriends;
            }

            set
            {
                this.onlineFriends = value;
            }
        }

        public List<UserInformation> FriendshipOffers
        {
            get
            {
                return this.friendshipOffers;
            }

            set
            {
                this.friendshipOffers = value;
            }
        }

        public List<UserInformation> Subscribers
        {
            get
            {
                return this.subscribers;
            }

            set
            {
                this.subscribers = value;
            }
        }

        public List<News> News
        {
            get
            {
                return this.news;
            }

            set
            {
                this.news = value;
            }
        }
    }
}