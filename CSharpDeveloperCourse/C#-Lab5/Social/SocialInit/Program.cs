namespace SocialInit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Social.Models;
    using Newtonsoft.Json;
    
    public class Program
    {
        private const string PathDirectory = @"../../../DataTemplate";
        private const string PathUsers = PathDirectory + "/users.json";
        private const string PathFriends = PathDirectory + "/friends.json";
        private const string PathMessages = PathDirectory + "/messages.json";

        public static void Main(string[] args)
        {
            using (StreamWriter outputFile = new StreamWriter(PathUsers))
            {
                var rnd = new Random();

                outputFile.WriteLine("[");

                for (var i = 0; i < 100; i++)
                {
                    var dateBirth = new DateTime(rnd.Next(1970, 2002), rnd.Next(1, 13), rnd.Next(1, 28));
                    var dateLastVisit = new DateTime(2018, rnd.Next(1, 13), rnd.Next(1, 28));
                    var name = "User" + i.ToString();
                    var user = new User(dateBirth, rnd.Next(0, 2), dateLastVisit, name, Convert.ToBoolean(rnd.Next(0, 2)), i);

                    var serealized = JsonConvert.SerializeObject(user);

                    if (i != 99)
                    {
                        serealized = serealized + ",";
                    }

                    outputFile.WriteLine(serealized);
                }

                outputFile.WriteLine("]");
            }

            using (StreamWriter outputFile = new StreamWriter(PathFriends))
            {
                var rnd = new Random();

                outputFile.WriteLine("[");

                for (var i = 0; i < 1000; i++)
                {
                    var fromUserId = rnd.Next(0, 100);
                    var toUserId = rnd.Next(0, 100);

                    while (fromUserId == toUserId)
                    {
                        toUserId = rnd.Next(0, 100);
                    }

                    var status = rnd.Next(0, 4);
                    var dateSend = new DateTime(2018, rnd.Next(1, 13), rnd.Next(1, 28));

                    var friend = new Friend(fromUserId, dateSend, status, toUserId);

                    var serealized = JsonConvert.SerializeObject(friend);

                    if (i != 999)
                    {
                        serealized = serealized + ",";
                    }

                    outputFile.WriteLine(serealized);
                }

                outputFile.WriteLine("]");
            }

            using (StreamWriter outputFile = new StreamWriter(PathMessages))
            {
                var rnd = new Random();

                outputFile.WriteLine("[");

                for (var i = 0; i < 200; i++)
                {
                    var fromUserId = rnd.Next(0, 100);
                    var messageId = i;

                    var dateSend = new DateTime(2018, rnd.Next(1, 13), rnd.Next(1, 28));
                    var text = "Hi! I'm a User" + fromUserId + ". How are you?";

                    var likesAmount = rnd.Next(0, 26);
                    var likes = new List<int>();
                    for (var k = 0; k < likesAmount; k++)
                    {
                        var userId = rnd.Next(0, 100);
                        while (likes.Contains(userId))
                        {
                            userId = rnd.Next(0, 100);
                        }

                        likes.Add(userId);
                    }

                    var message = new Message(fromUserId, likes, messageId, dateSend, text);

                    var serealized = JsonConvert.SerializeObject(message);

                    if (i != 199)
                    {
                        serealized = serealized + ",";
                    }

                    outputFile.WriteLine(serealized);
                }

                outputFile.WriteLine("]");
            }
        }
    }
}