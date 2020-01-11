namespace ColorReplacement
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var regex = new Regex(@"(rgb\(([0-2]?[0-9]?[0-9]?\,?\s*){3}\))|(#[(a-f)(A-F)(0-9)]{6})|(#[(a-f)(A-F)(0-9)]{3}\s)");

            var colorsDictionary = new Dictionary<string, string>();

            using (var colorsReader = new StreamReader(@"..\..\Data\colors.txt"))
            {
                string line;

                while ((line = colorsReader.ReadLine()) != null)
                {
                    var tmp = line.Split(' ');
                    colorsDictionary.Add(tmp[1], tmp[0]);
                }
            }

            using (var sourceReader = new StreamReader(@"..\..\Data\source.txt"))
            {
                var text = new StringBuilder();
                text.Append(File.ReadAllText(@"..\..\Data\source.txt"));
                var colors = new SortedList();
                string line;

                while ((line = sourceReader.ReadLine()) != null)
                {
                    var match = regex.Match(line);

                    while (match.Success)
                    { 
                        var hexCode = GetHexColorCode(match.Value);

                        if (colorsDictionary.ContainsKey(hexCode))
                        {
                            string colorName;
                            colorsDictionary.TryGetValue(hexCode, out colorName);

                            text.Replace(match.Value, colorName);

                            if (!colors.Contains(hexCode))
                            {
                                colors.Add(hexCode, colorName);
                            }
                        }

                        match = match.NextMatch();
                    }
                }

                using (var sw = new StreamWriter(@"used_colors.txt", false, System.Text.Encoding.Default))
                {
                    for (var i = 0; i < colors.Count; i++)
                    {
                        sw.WriteLine(colors.GetKey(i) + " " + colors.GetByIndex(i));
                    }
                }

                File.WriteAllText(@"target.txt", text.ToString());

                Console.ReadKey();
            }
        }

        public static string GetHexColorCode(string colorCode)
        {
            string hexCode;

            // Сheck what type the color code has.
            if (colorCode[0] == '#')
            {
                if (colorCode.Length < 6)
                {
                    // Convert short hex-color to standart hex-color.
                    hexCode = "#" + colorCode[1] + colorCode[1] + colorCode[2] + colorCode[2] + colorCode[3] +
                              colorCode[3];
                    hexCode = hexCode.ToUpper();

                    return hexCode;
                }

                hexCode = colorCode.ToUpper();

                return hexCode;
            }

            // Convert rgb-color to hex-color.
            var tmp = Regex.Replace(colorCode, @"rgb\(", string.Empty).Trim(')');
            tmp = Regex.Replace(tmp, @"\s", string.Empty);

            string[] rgbArray = tmp.Split(',');
            byte[] rgbCode =
            {
                Convert.ToByte(rgbArray[0]), Convert.ToByte(rgbArray[1]), Convert.ToByte(rgbArray[2])
            };

            hexCode = "#" + rgbCode[0].ToString("X2") + rgbCode[1].ToString("X2") + rgbCode[2].ToString("X2");

            return hexCode;
        }
    }
}
