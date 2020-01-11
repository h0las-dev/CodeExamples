namespace WordCount
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var cyrillicEncoding = Encoding.GetEncoding(1251);

                var textPart1 = File.ReadAllText(@"..\..\Data\Беда_одна_не_ходит.txt", cyrillicEncoding);
                var textPart2 = File.ReadAllText(@"..\..\Data\Начало.txt", cyrillicEncoding);
                var textPart3 = File.ReadAllText(@"..\..\Data\Хэппи_Энд.txt", cyrillicEncoding);
                var fullText = textPart1 + textPart2 + textPart3;

                var word = string.Empty;
                var wordsSet = new HashSet<string>();

                foreach (char letterOrDigit in fullText)
                {
                    if (char.IsDigit(letterOrDigit) || char.IsLetter(letterOrDigit))
                    {
                        word = word + letterOrDigit;
                    }
                    else
                    {
                        if (word != string.Empty)
                        {
                            word = word.ToLower();
                            wordsSet.Add(word);
                            word = string.Empty;
                        }
                    }
                }

                Console.WriteLine("The amount of unique words is: {0}", wordsSet.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
