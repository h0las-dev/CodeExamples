namespace SecretBook
{
    using System;
    using System.IO;
    using RJEncryption;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var dataProducer = new DataProducer();

                var path = @"..\..\Data\Notes.txt";

                var passwordKey = string.Empty;

                Console.WriteLine("Enter a password(at least 8 characters, without backspace): ");
                while (true)
                {
                    ConsoleKeyInfo consoleKey = Console.ReadKey(true);

                    if (consoleKey.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("\n");
                        break;
                    }
                    else if (consoleKey.Key == ConsoleKey.Spacebar)
                    {
                        throw new ArgumentException("Error: the password can't contain spaces");
                    }
                    else if (consoleKey.Key == ConsoleKey.Backspace)
                    {
                        passwordKey = passwordKey.Remove(passwordKey.Length - 1);
                        Console.Write("\b \b");
                    }
                    else
                    {
                        passwordKey += consoleKey.KeyChar;
                        Console.Write("#");
                    }
                }

                if (passwordKey.Length < 8)
                {
                    throw new ArgumentException("Error: the password must contain more than 8 characters!");
                }

                string data = File.ReadAllText(path);
                string ciperData = string.Empty;

                if (data == string.Empty)
                {
                    dataProducer.ShowMenu();
                }
                else
                {
                    string decData = RJEncryptor.Decrypt(data, passwordKey);

                    dataProducer.DecryptData = decData;
                    dataProducer.ParseNotes();
                    dataProducer.ShowMenu();
                }

                ciperData = RJEncryptor.Encrypt(dataProducer.DecryptData, passwordKey);

                File.WriteAllText(path, ciperData);

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}