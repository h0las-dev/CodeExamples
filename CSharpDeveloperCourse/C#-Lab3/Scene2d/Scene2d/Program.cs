namespace Scene2d
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using Exceptions;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();
            var lineCounter = 0;
            var commentRegex = new Regex(@"^\s*#.+");

            try
            {
                foreach (var line in File.ReadLines(@"..\..\..\Data\input.txt"))
                {
                    lineCounter++;

                    if (commentRegex.IsMatch(line))
                    {
                        continue;
                    }

                    if (line == null || line.Trim().Length == 0)
                    {
                        break;
                    }

                    commandProducer.AppendLine(line);

                    if (commandProducer.IsCommandReady)
                    {
                        var command = commandProducer.GetCommand();
                        command.Apply(scene);
                    }
                }
            }
            catch (BadFormatException ex)
            {
                Console.WriteLine("error in line {0}: {1}", lineCounter, ex.Message);
            }
            catch (BadNameExeption ex)
            {
                Console.WriteLine("error in line {0}: {1}", lineCounter, ex.Message);
            }
            catch (BadPolygonPointExeption ex)
            {
                Console.WriteLine("error in line {0}: {1}", lineCounter, ex.Message);
            }
            catch (BadCircleRadiusExeption ex)
            {
                Console.WriteLine("error in line {0}: {1}", lineCounter, ex.Message);
            }
            catch (BadPolygonPointNumberExeption ex)
            {
                Console.WriteLine("error in line {0}: {1}", lineCounter, ex.Message);
            }
            catch (BadRectanglePointExeption ex)
            {
                Console.WriteLine("error in line {0}: {1}", lineCounter, ex.Message);
            }
            catch (NameDoesAlreadyExistExeption ex)
            {
                Console.WriteLine("error in line {0}: {1}", lineCounter, ex.Message);
            }
            catch (UnexpectedEndOfPolygonExeption ex)
            {
                Console.WriteLine("error in line {0}: {1}", lineCounter, ex.Message);
            }

            Console.ReadKey();
        }
    }
}