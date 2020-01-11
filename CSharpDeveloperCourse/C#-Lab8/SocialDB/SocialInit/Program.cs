namespace SocialInit
{
    using System;
    using System.Collections.Generic;
    using SocialInit.Properties;

    public class Program
    {
        // Path for j-son generator.
        /*private const string PathDirectory = @"../../../DataTemplate";
        private const string PathUsers = PathDirectory + "/users.json";
        private const string PathFriends = PathDirectory + "/friends.json";
        private const string PathMessages = PathDirectory + "/messages.json";*/

        public static void Main(string[] args)
        {
            var dataGenerator = new DataGenerator(Settings.Default.ConnectionString);

            var rnd = new Random();

            for (var i = 0; i < 100; i++)
            {
                var dateBirth = new DateTime(rnd.Next(1970, 2002), rnd.Next(1, 13), rnd.Next(1, 28));
                var dateLastVisit = new DateTime(2018, rnd.Next(1, 13), rnd.Next(1, 28));

                var name = "User" + i.ToString();

                var gender = i % 2;
                var status = i % 2;

                dataGenerator.AddUser(gender, dateBirth, dateLastVisit, status, name);
            }

            rnd = new Random();

            for (var i = 0; i < 1000; i++)
            {
                Console.WriteLine(i);
                var fromUserId = rnd.Next(1, 100);
                var toUserId = rnd.Next(1, 100);

                while (fromUserId == toUserId)
                {
                    toUserId = rnd.Next(1, 100);
                }

                var status = rnd.Next(0, 4);
                var dateSend = new DateTime(2018, rnd.Next(1, 13), rnd.Next(1, 28));

                dataGenerator.AddFriends(fromUserId, toUserId, status, dateSend);
            }

            rnd = new Random();

            for (var i = 1; i <= 200; i++)
            {
                var fromUserId = rnd.Next(1, 100);
                var messageId = i;

                var dateSend = new DateTime(2018, rnd.Next(1, 13), rnd.Next(1, 28));
                var text = "Hi! I'm a User" + fromUserId + ". How are you?";

                dataGenerator.AddMessage(fromUserId, dateSend, text);

                var likesAmount = rnd.Next(0, 26);
                var likes = new List<int>();
                for (var k = 0; k < likesAmount; k++)
                {
                    var userId = rnd.Next(1, 100);
                    while (likes.Contains(userId))
                    {
                        userId = rnd.Next(1, 100);
                    }

                    dataGenerator.AddLikes(userId, messageId);
                }
            }
        }
    }
}