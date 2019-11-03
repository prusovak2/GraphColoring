using System;
using System.IO;
using System.Collections.Generic;

namespace Resulnterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!OpenFile(args, out StreamReader Input, out int NumOfColors))
            {
                return;
            }

            string[] Line = ProcessLine(Input);
            if (Line == null)
            {
                Console.WriteLine("Epmty input file");
                return;
            }

            List<int>[] result = new List<int>[NumOfColors];
            for (int  i = 0;  i <NumOfColors;  i++)
            {
                result[i] = new List<int>();
            }
            
            for (int i = 0; i < Line.Length-1; i++)
            {
                if(!Int32.TryParse(Line[i],out int LogVar))
                {
                    Console.WriteLine("Syntax Error");
                    return;
                }
                if (LogVar > 0)
                {
                    LogVar--;
                    int color = LogVar % NumOfColors;
                    int vertex = (LogVar / NumOfColors) + 1;
                    result[color].Add(vertex);
                }                
            }

            foreach (var ListOfVertices in result)
            {
                for (int i = 0; i < ListOfVertices.Count-1; i++)
                {
                    Console.Write($"{ListOfVertices[i]} ");
                }
                Console.WriteLine($"{ListOfVertices[ListOfVertices.Count-1]}");
            }
        }
        private static bool OpenFile(string[] args, out StreamReader Reader, out int NumOfColors)
        {
            Reader = null;
            NumOfColors = 0;
            if (args.Length != 2)
            {
                Console.WriteLine("Error: incorect number of arguments.");
                return false;
            }
            try
            {
                NumOfColors = Convert.ToInt32(args[1]);
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("Error: the second argument must be a natural number.");
                return false;
            }
            if (NumOfColors <= 0)
            {
                Console.WriteLine("Error: the second argument must be a natural number.");
                return false;
            }
            try
            {
                Reader = new StreamReader(args[0]);
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is FileNotFoundException || e is DirectoryNotFoundException || e is IOException)
            {
                Console.WriteLine("Error: input file cannot be opened.");
                return false;
            }
            return true;

        }
         static string[] ProcessLine(StreamReader Input)
        {
            string line = Input.ReadLine();
            if (line != null)
            {
                return (line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                return null;
            }
        }



    }
}
