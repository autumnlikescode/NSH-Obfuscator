using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NSH_Obfuscator
{
    internal class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.Title = "NSH Obfuscator by autumn4190";
            // Provide the path to the input text file
            Console.Write("Enter Input File Path: ");
            string inputFile = Console.ReadLine();

            if (inputFile.Contains("\""))
            {
                inputFile = inputFile.Replace("\"", "");
            }

            // Create a directory named "obfuscated" inside the same directory as the original file
            string outputFolder = Path.Combine(Path.GetDirectoryName(inputFile), "obfuscated");
            Directory.CreateDirectory(outputFolder);

            // Get the file name without extension from the input file path
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile);

            // Path to save the modified file inside the "obfuscated" folder
            string outputFile = Path.Combine(outputFolder, $"{fileNameWithoutExtension}.nsh");

            ConsoleSpinner spinner = new ConsoleSpinner();
            spinner.Delay = 300;

            Console.ForegroundColor = ConsoleColor.Green; // Set console text color to green
            Console.WriteLine("Processing...");

            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Read the original file content
            string[] originalLines = File.ReadAllLines(inputFile);

            // Generate random junk lines
            List<string> junkLines = GenerateJunkLines(40, 500); // Change 5 and 20 to customize the number and length of junk lines

            // Create a list to store modified content
            List<string> modifiedLines = new List<string>();

            // Add junk lines before and after each existing line in the input file
            foreach (var line in originalLines)
            {
                modifiedLines.AddRange(junkLines);
                modifiedLines.Add($"{line} {junkLines[0]}"); // Adding the first junk line only once next to each line
                modifiedLines.AddRange(junkLines);
            }

            // Save the modified content to a new file inside the "obfuscated" folder
            File.WriteAllLines(outputFile, modifiedLines);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.ForegroundColor = ConsoleColor.Yellow; // Set console text color to yellow
            Console.WriteLine("File modification completed. Modified content saved to obfuscated folder.");
            Console.WriteLine($"Time taken: {elapsedMs} milliseconds");
            Console.ReadKey();
        }

        static List<string> GenerateJunkLines(int numberOfLines, int lineLength)
        {
            List<string> junkLines = new List<string>();
            Random random = new Random();
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (int i = 0; i < numberOfLines; i++)
            {
                StringBuilder junkLine = new StringBuilder("#");
                for (int j = 0; j < lineLength; j++)
                {
                    junkLine.Append(characters[random.Next(characters.Length)]);
                }
                junkLines.Add(junkLine.ToString());
            }

            return junkLines;
        }

        public class ConsoleSpinner
        {
            static string[,] sequence = null;

            public int Delay { get; set; } = 200;

            int totalSequences = 0;
            int counter;

            public ConsoleSpinner()
            {
                counter = 0;
                sequence = new string[,] {
                { "/", "-", "\\", "|" },
                { ".", "o", "0", "o" },
                { "+", "x","+","x" },
                { "V", "<", "^", ">" },
                { ".   ", "..  ", "... ", "...." },
                { "=>   ", "==>  ", "===> ", "====>" },
               // ADD YOUR OWN CREATIVE SEQUENCE HERE IF YOU LIKE
            };

                totalSequences = sequence.GetLength(0);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sequenceCode"> 0 | 1 | 2 |3 | 4 | 5 </param>
            public void Turn(string displayMsg = "", int sequenceCode = 0)
            {
                counter++;

                Thread.Sleep(Delay);

                sequenceCode = sequenceCode > totalSequences - 1 ? 0 : sequenceCode;

                int counterValue = counter % 4;

                string fullMessage = displayMsg + sequence[sequenceCode, counterValue];
                int msglength = fullMessage.Length;

                Console.Write(fullMessage);

                Console.SetCursorPosition(Console.CursorLeft - msglength, Console.CursorTop);
            }
        }
    }
}
