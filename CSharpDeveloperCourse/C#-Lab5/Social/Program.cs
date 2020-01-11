namespace Social
{
    using System;

    public class Program
    {
        private const string PathDirectory = @"../../DataTemplate";
        private const string PathUsers = PathDirectory + "/users.json";
        private const string PathFriends = PathDirectory + "/friends.json";
        private const string PathMessages = PathDirectory + "/messages.json";

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Имя пользователя не задано!");
                Console.ReadKey();
                return;
            }

            var name = args[0];

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception();
            }

            var socialDataSource = new SocialDataSource(PathUsers, PathFriends, PathMessages);

            var userContext = socialDataSource.GetUserContext(name);

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;

            do
            {
                Console.WriteLine("Привет, {0}!\n{1} - года(лет)\n", userContext.User.Name, DateTime.Today.Year - userContext.User.DateofBirth.Year);
                Console.WriteLine("[1] Друзья:\n[2] Друзья онлайн:\n[3] Заявки в друзья:\n[4] Подписчики:\n[5] Новости:\n[6] Выход:");

                var i = 0;

                try
                {
                    i = int.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
                
                switch (i)
                {
                    case 1:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Список друзей({0}):\n", userContext.Friends.Count);
                        foreach (var friend in userContext.Friends)
                        {
                            Console.WriteLine("{0,-15} | {1,10}", friend.UserName, friend.Online);
                        }

                        Console.WriteLine();
                        break;
                    case 2:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Список друзей онлайн({0}):\n", userContext.OnlineFriends.Count);
                        foreach (var friend in userContext.OnlineFriends)
                        {
                            Console.WriteLine("{0,-15} | {1,10}", friend.UserName, friend.Online);
                        }

                        Console.WriteLine();
                        break;
                    case 3:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Заявки в друзья({0}):\n", userContext.Subscribers.Count);
                        foreach (var friend in userContext.Subscribers)
                        {
                            Console.WriteLine("{0,-15} | {1,10}", friend.UserName, friend.Online);
                        }

                        Console.WriteLine();
                        break;
                    case 4:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Подписчики({0}):\n", userContext.FriendshipOffers.Count);
                        foreach (var friend in userContext.FriendshipOffers)
                        {
                            Console.WriteLine("{0,-15} | {1,10}", friend.UserName, friend.Online);
                        }

                        Console.WriteLine();
                        break;
                    case 5:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Новости:\n");
                        foreach (var message in userContext.News)
                        {
                            Console.WriteLine("{0,-50} | {1,10} | likes: {2,10}", message.Text, message.AuthorName, message.Likes.Count);
                        }

                        Console.WriteLine();
                        break;
                    case 6:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Выход...");
                        return;
                    default:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Ошибка ввода...");
                        break;
                }
            }
            while (true);
        }
    }
}