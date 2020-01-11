namespace Casting
{
    using System;

    public class Program
    {
        public enum Weekday : byte
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday = 16,
            Sunday
        }

        [Flags]
        public enum BookAttributes : short
        {
            IsNothing = 0x0,
            IsEducational = 0x1,
            IsDetective = 0x2,
            IsHumoros = 0x4,
            IsMedical = 0x8,
            IsPolitical = 0x10,
            IsEconomical = 0x20
        }

        public static void Main(string[] args)
        {
            try
            {
                var number = Convert.ToInt16(Console.ReadLine());

                var week = (Weekday)number;
                var attributes = (BookAttributes)number;

                Console.WriteLine("{0};\n{1};", week, attributes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}