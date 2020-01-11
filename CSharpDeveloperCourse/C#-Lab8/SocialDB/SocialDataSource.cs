namespace Social
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using Models;
    
    public class SocialDataSource
    {
        private readonly string connectionString;

        private List<User> users;

        private List<Friend> friends;

        private List<Message> messages;

        public SocialDataSource(string conectString)
        {
            this.Users = new List<User>();
            this.Friends = new List<Friend>();
            this.Messages = new List<Message>();

            this.connectionString = conectString;
        }

        public List<User> Users
        {
            get
            {
                return this.users;
            }

            set
            {
                this.users = value;
            }
        }

        public List<Friend> Friends
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

        public List<Message> Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.messages = value;
            }
        }

        public UserContext GetUserContext(string userName)
        {
            var userContext = new UserContext();

            userContext.User = this.GetUser(userName);

            userContext.Friends = this.GetUserFriends(userContext.User);

            userContext.OnlineFriends = this.GetUserOnlineFriends(userContext.Friends);

            userContext.Subscribers = this.GetUserSubscribers(userContext.User);

            userContext.FriendshipOffers = this.GetFriendshipOffers(userContext.User);

            userContext.News = this.GetMessages(userContext.Friends);

            return userContext;
        }

        public void ReadAllusers()
        {
            this.Users.Clear();

            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                // Console.WriteLine("Connection success: " + connection.ConnectionString);
                var comand = new SqlCommand("SELECT * FROM [Users]", connection);
                using (var dataReader = comand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var dateBirth = (DateTime)dataReader["dateOfBirth"];
                        var dateLastVisit = (DateTime)dataReader["lastVisit"];
                        var gender = Convert.ToInt32(dataReader["gender"]);
                        var name = (string)dataReader["name"];
                        var isOnline = Convert.ToInt32(dataReader["isOnline"]);
                        var userId = (int)dataReader["userId"];

                        this.Users.Add(new User(dateBirth, gender, dateLastVisit, name, isOnline, userId));
                    }
                }
            }
        }

        public void ReadAllFriends()
        {
            this.Friends.Clear();

            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                // Console.WriteLine("Connection success: " + connection.ConnectionString);
                var comand = new SqlCommand("SELECT * FROM [Friends]", connection);
                using (var dataReader = comand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var sendDate = (DateTime)dataReader["sendDate"];
                        var friendStatus = Convert.ToInt32(dataReader["friendStatus"]);
                        var userFrom = (int)dataReader["userFrom"];
                        var userTo = (int)dataReader["userTo"];

                        this.Friends.Add(new Friend(userFrom, sendDate, friendStatus, userTo));
                    }
                }
            }
        }

        public void ReadAllLikes()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                // Console.WriteLine("Connection success: " + connection.ConnectionString);
                var comand = new SqlCommand("SELECT * FROM [Likes]", connection);
                using (var dataReader = comand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var userId = (int)dataReader["userId"];
                        var messageId = (int)dataReader["messageId"];

                        foreach (var msg in this.Messages)
                        {
                            if (msg.MessageId == messageId)
                            {
                                msg.Likes.Add(userId);
                            }
                        }
                    }
                }
            }
        }

        public void ReadAllMessage()
        {
            this.Messages.Clear();

            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                // Console.WriteLine("Connection success: " + connection.ConnectionString);
                var comand = new SqlCommand("SELECT * FROM [Messages]", connection);
                using (var dataReader = comand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var messageId = (int)dataReader["messageId"];
                        var sendDate = (DateTime)dataReader["sendDate"];
                        var authorId = Convert.ToInt32(dataReader["authorId"]);
                        var messageText = (string)dataReader["messageText"];

                        var userLikers = new List<int>();

                        this.Messages.Add(new Message(authorId, userLikers, messageId, sendDate, messageText));
                    }
                }
            }
         }

        private User GetUser(string name)
        {
            var tmpUser = this.Users.Where(user => user.Name == name).Select(user => user);

            return tmpUser.First();
        }

        private List<UserInformation> GetUserFriends(User currentUser)
        {
            var symmetryFriends1 = this.Friends.Where(
                user => user.ToUserId == currentUser.UserId && user.Status != 3).Select(user => user);

            var symmetryFriends2 = this.Friends.Where(
                user => user.FromUserId == currentUser.UserId && user.Status != 3).Select(user => user);

            var symmetryFriends = symmetryFriends1.Join(
                symmetryFriends2,
                user1 => user1.FromUserId,
                user2 => user2.ToUserId,
                (user1, user2) => user1);

            var trueFriends = this.Friends.Where(friend =>
                ((friend.ToUserId == currentUser.UserId || friend.FromUserId == currentUser.UserId) &&
                 friend.Status == 2)).Select(friend => friend);

            var currentUserFriends = new List<Friend>(symmetryFriends.Union(trueFriends).Distinct());

            var friendsInfo1 = currentUserFriends.Where(friend => friend.FromUserId != currentUser.UserId).Join(
                this.Users,
                friend => friend.FromUserId,
                friendInfo => friendInfo.UserId,
                (friend, friendInfo) => new UserInformation(friendInfo.Name, friendInfo.Online, friendInfo.UserId));

            var friendsInfo2 = currentUserFriends.Where(friend => friend.ToUserId != currentUser.UserId).Join(
                this.Users,
                friend => friend.ToUserId,
                friendInfo => friendInfo.UserId,
                (friend, friendInfo) => new UserInformation(friendInfo.Name, friendInfo.Online, friendInfo.UserId));

            return new List<UserInformation>(friendsInfo1.Union(friendsInfo2).Distinct());
        }

        private List<UserInformation> GetUserSubscribers(User currentUser)
        {
            var subscribers = this.Friends.Where(friend =>
                ((friend.ToUserId == currentUser.UserId) &&
                 (friend.Status == 0 || friend.Status == 1))).Select(friend => friend);

            subscribers = subscribers.Distinct();

            var friendsInfo = subscribers.Join(
                this.Users,
                subscriber => subscriber.FromUserId,
                subscriberInfo => subscriberInfo.UserId,
                (subscriber, subscriberInfo) => new UserInformation(subscriberInfo.Name, subscriberInfo.Online, subscriberInfo.UserId));

            return new List<UserInformation>(friendsInfo);
        }

        private List<UserInformation> GetUserOnlineFriends(List<UserInformation> friends)
        {
            return new List<UserInformation>(friends.Where(friend => friend.Online == 1));
        }

        private List<UserInformation> GetFriendshipOffers(User currentUser)
        {
            var offers = this.Friends.Where(
                subscriber =>
                    (subscriber.ToUserId == currentUser.UserId && subscriber.SendDate >= currentUser.LastVisir && subscriber.Status == 0)).Join(
                this.Users,
                subscriber => subscriber.FromUserId,
                subscriberInfo => subscriberInfo.UserId,
                (subscriber, subscriberInfo) =>
                    new UserInformation(subscriberInfo.Name, subscriberInfo.Online, subscriberInfo.UserId));

            offers.Distinct();

            return new List<UserInformation>(offers);
        }

        private List<News> GetMessages(List<UserInformation> friends)
        {
            var friendMessages = friends.Join(
                this.Messages,
                friend => friend.UserId,
                message => message.AuthorId,
                (friend, message) =>
                    new Message(message.AuthorId, message.Likes, message.MessageId, message.SendDate, message.Text));

            var news = friendMessages.Join(
                this.Users,
                message => message.AuthorId,
                user => user.UserId,
                (message, user) => new News(user.UserId, user.Name, message.Likes, message.Text));

            return new List<News>(news);
        }

        // Methods for j-son generator.
        /*private void GetUsers(string path)
        {
            string jsonUsersFile = File.ReadAllText(path);

            User[] usersArray = JsonConvert.DeserializeObject<User[]>(jsonUsersFile);

            this.users.AddRange(usersArray);
        }

        private void GetFriends(string path)
        {
            string jsonFriendsFile = File.ReadAllText(path);

            Friend[] friendsArray = JsonConvert.DeserializeObject<Friend[]>(jsonFriendsFile);

            this.friends.AddRange(friendsArray);
        }

        private void GetMessages(string path)
        {
            string jsonMessagesFile = File.ReadAllText(path);

            Message[] messagesArray = JsonConvert.DeserializeObject<Message[]>(jsonMessagesFile);

            this.messages.AddRange(messagesArray);
        }*/
    }
}