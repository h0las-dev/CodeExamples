namespace Social
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;
    using Newtonsoft.Json;

    public class SocialDataSource
    {
        private List<User> users;

        private List<Friend> friends;

        private List<Message> messages;

        public SocialDataSource(string pathUsers, string pathFriends, string pathMessages)
        {
            this.users = new List<User>();
            this.friends = new List<Friend>();
            this.messages = new List<Message>();

            this.GetUsers(pathUsers);
            this.GetFriends(pathFriends);
            this.GetMessages(pathMessages);
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

        private User GetUser(string name)
        {
            var tmpUser = this.users.Where(user => user.Name == name).Select(user => user);

            return tmpUser.First();
        }

        private List<UserInformation> GetUserFriends(User currentUser)
        {
            var symmetryFriends1 = this.friends.Where(
                user => user.ToUserId == currentUser.UserId && user.Status != 3).Select(user => user);

            var symmetryFriends2 = this.friends.Where(
                user => user.FromUserId == currentUser.UserId && user.Status != 3).Select(user => user);

            var symmetryFriends = symmetryFriends1.Join(
                symmetryFriends2,
                user1 => user1.FromUserId,
                user2 => user2.ToUserId,
                (user1, user2) => user1);

            var trueFriends = this.friends.Where(friend =>
                ((friend.ToUserId == currentUser.UserId || friend.FromUserId == currentUser.UserId) &&
                 friend.Status == 2)).Select(friend => friend);

            var currentUserFriends = new List<Friend>(symmetryFriends.Union(trueFriends).Distinct());

            var friendsInfo1 = currentUserFriends.Where(friend => friend.FromUserId != currentUser.UserId).Join(
                this.users,
                friend => friend.FromUserId,
                friendInfo => friendInfo.UserId,
                (friend, friendInfo) => new UserInformation(friendInfo.Name, friendInfo.Online, friendInfo.UserId));

            var friendsInfo2 = currentUserFriends.Where(friend => friend.ToUserId != currentUser.UserId).Join(
                this.users,
                friend => friend.ToUserId,
                friendInfo => friendInfo.UserId,
                (friend, friendInfo) => new UserInformation(friendInfo.Name, friendInfo.Online, friendInfo.UserId));

            return new List<UserInformation>(friendsInfo1.Union(friendsInfo2).Distinct());
        }

        private List<UserInformation> GetUserSubscribers(User currentUser)
        {
            var subscribers = this.friends.Where(friend =>
                ((friend.ToUserId == currentUser.UserId) &&
                 (friend.Status == 0 || friend.Status == 1))).Select(friend => friend);

            subscribers = subscribers.Distinct();

            var friendsInfo = subscribers.Join(
                this.users,
                subscriber => subscriber.FromUserId,
                subscriberInfo => subscriberInfo.UserId,
                (subscriber, subscriberInfo) => new UserInformation(subscriberInfo.Name, subscriberInfo.Online, subscriberInfo.UserId));

            return new List<UserInformation>(friendsInfo);
        }

        private List<UserInformation> GetUserOnlineFriends(List<UserInformation> friends)
        {
            return new List<UserInformation>(friends.Where(friend => friend.Online == true));
        }

        private List<UserInformation> GetFriendshipOffers(User currentUser)
        {
            var offers = this.friends.Where(
                subscriber =>
                    (subscriber.ToUserId == currentUser.UserId && subscriber.SendDate >= currentUser.LastVisir && subscriber.Status == 0)).Join(
                this.users,
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
                this.messages,
                friend => friend.UserId,
                message => message.AuthorId,
                (friend, message) =>
                    new Message(message.AuthorId, message.Likes, message.MessageId, message.SendDate, message.Text));

            var news = friendMessages.Join(
                this.users,
                message => message.AuthorId,
                user => user.UserId,
                (message, user) => new News(user.UserId, user.Name, message.Likes, message.Text));

            return new List<News>(news);
        }

        private void GetUsers(string path)
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
        }
    }
}