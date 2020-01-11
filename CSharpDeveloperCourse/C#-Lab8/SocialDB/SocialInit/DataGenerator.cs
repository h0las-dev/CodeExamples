namespace SocialInit
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Social.Models;

    public class DataGenerator
    {
        private readonly string connectionString;

        private List<User> users;
        private List<Friend> friends;
        private List<Message> messages;

        public DataGenerator(string connectString)
        {
            this.connectionString = connectString;

            this.Users = new List<User>();
            this.Friends = new List<Friend>();
            this.Messages = new List<Message>();
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

        public void AddUser(int gender, DateTime dob, DateTime lv, int status, string userName)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand("dataBaseAddUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@gend", gender);
                command.Parameters.AddWithValue("@dob", dob.ToString());
                command.Parameters.AddWithValue("@lv", lv.ToString());
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@userName", userName);

                var returnParam = command.Parameters.Add("@NewUserId", SqlDbType.Int);

                returnParam.Direction = ParameterDirection.ReturnValue;
                var rowCount = command.ExecuteNonQuery();

                if (rowCount == 1)
                {
                    var userId = (int)returnParam.Value;
                    this.Users.Add(new User(dob, gender, lv, userName, status, userId));
                }
            }
        }

        public void AddFriends(int userFrom, int userTo, int friendStatus, DateTime date)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand("dataBaseAddFriends", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@fromId", userFrom);
                command.Parameters.AddWithValue("@toId", userTo);
                command.Parameters.AddWithValue("@status", friendStatus);
                command.Parameters.AddWithValue("@date", date.ToString());

                var returnParam = command.Parameters.Add("@NewFriendId", SqlDbType.Int);

                returnParam.Direction = ParameterDirection.ReturnValue;
                var rowCount = command.ExecuteNonQuery();

                if (rowCount == 1)
                {
                    var friendId = (int)returnParam.Value;
                    this.Friends.Add(new Friend(userFrom, date, friendStatus, userTo));
                }
            }
        }

        public void AddLikes(int user, int messageId)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand("dataBaseAddLike", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@user", user);
                command.Parameters.AddWithValue("@message", messageId);

                var rowCount = command.ExecuteNonQuery();

                if (rowCount == 1)
                {
                    foreach (var msg in this.Messages)
                    {
                        if (msg.MessageId == messageId)
                        {
                            msg.Likes.Add(user);
                        }
                    }
                }
            }
        }

        public void AddMessage(int user, DateTime date, string message)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand("dataBaseAddMessage", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@author", user);
                command.Parameters.AddWithValue("@date", date.ToString());
                command.Parameters.AddWithValue("@message", message);

                var returnParam = command.Parameters.Add("@NewMessageId", SqlDbType.Int);

                returnParam.Direction = ParameterDirection.ReturnValue;
                var rowCount = command.ExecuteNonQuery();

                if (rowCount == 1)
                {
                    var messageId = (int)returnParam.Value;

                    var likers = new List<int>();

                    this.Messages.Add(new Message(user, likers, messageId, date, message));
                }
            }
        }
    }
}