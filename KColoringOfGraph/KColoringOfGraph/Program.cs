using System;
using System.IO;

namespace KColoringOfGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!OpenFile(args, out StreamReader Input, out int NumOfColors))
            {
                return;
            }
            FormulaGenerator Generator = new FormulaGenerator(NumOfColors, Input);
            if (Generator.Generate())
            {
                Generator.PrintOutput();
            }

        }

        
        /// <summary>
        /// checks validity of arguments and opens input file
        /// </summary>
        /// <param name="args"></param>
        /// <param name="Reader"></param>
        /// <param name="NumOfColors"></param>
        /// <returns>true if arguments are valid and file can be opened, false otherwise</returns>
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
    }
  
}
