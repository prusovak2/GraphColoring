using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KColoringOfGraph
{
    class FormulaGenerator
    {
        private int NumOfColors;
        private int NumOfVertices;
        private int NumOfEdges;
        private StreamReader Input;
        private StringBuilder Output;

        public FormulaGenerator(int NumOfColors, StreamReader Input)
        {
            this.NumOfColors = NumOfColors;
            this.Input = Input;
            this.Output = new StringBuilder();
        }
       /// <summary>
       /// generates cnf formula that hods if and only if an input graf can be colored by input number of colors
       /// </summary>
       /// <returns>false if theres is some syntax error in input file, true otherwise</returns>
        public bool Generate()
        {
            if (!this.ReadAndGenHead())
            {
                return false;
            }
            if (!this.CorrectColoring())
            {
                return false;
            }
            this.AtLeastOne();
            this.MaxOne();
            return true;
        }
         /// <summary>
         /// generates NumOfColor clauses for each edge, stating that both verteces doesn have the same color
         /// </summary>
         /// <returns>false if there si some syntax error in input file, false otherwise</returns>
        public bool CorrectColoring()
        {
            string[] line = this.ProcessLine();
            int counter = 1;
            //for ech line
            while (line != null && counter<=this.NumOfEdges)
            {
                if(line.Length!=3 || line[0] != "e")
                {
                    Console.WriteLine("Error: incorect format of line representing edge.");
                    return false;
                }
                if ((!Int32.TryParse(line[1], out int vertex1)) || (!Int32.TryParse(line[2], out int vertex2)))
                {
                    Console.WriteLine("Error: incorect format of line representing edge.");
                    return false;
                }
                for (int color = 1; color <= this.NumOfColors; color++)
                {
                    int a = this.CountVarNumber(vertex1, color, true);
                    int b = this.CountVarNumber(vertex2, color, true);
                    this.Output.AppendLine($"{a} {b} 0");
                }

                line = this.ProcessLine();
                counter++;
            }
            this.Input.Close();
            return true;
        }

        /// <summary>
        /// for each vertex and each pair of color generates clause, stating that vertex does not have two colors at once 
        /// </summary>
        public void MaxOne()
        {
            for (int vertex = 1; vertex <= this.NumOfVertices; vertex++)
            {
                for (int i = 1; i < this.NumOfColors; i++)
                {
                    for (int j = i+1; j <= this.NumOfColors; j++)
                    {
                        int a = this.CountVarNumber(vertex, i, true);
                        int b = this.CountVarNumber(vertex, j, true);
                        this.Output.AppendLine($"{a} {b} 0");
                    }
                }
            }
        }

        /// <summary>
        /// creates one clause for each vertex saying: vertex has at leats one color
        /// </summary>
        public void AtLeastOne()
        {
            for (int vertex = 1; vertex <= this.NumOfVertices; vertex++)
            {
                for (int color = 1; color <= this.NumOfColors ; color++)
                {
                    int v = this.CountVarNumber(vertex, color, false);
                    this.Output.Append($"{v} ");
                }
                this.Output.AppendLine("0");
            }
        }

        /// <summary>
        /// returns numeric representation of logical variable
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private int CountVarNumber(int vertex, int color, bool negative)
        {
            if ((vertex > this.NumOfVertices) || (color > this.NumOfColors))
            {
                Console.WriteLine("error my fault");
                return 0;
            }
            int VarNum = (vertex - 1) * this.NumOfColors + color;
            if (negative)
            {
                return (-1) * VarNum;
            }
            return VarNum;
        }
        /// <summary>
        /// processes a header of input file and generates header of output file
        /// </summary>
        /// <returns>true, if format of header is correct, false otherwise</returns>
        public bool ReadAndGenHead()
        {
            string[] Line = this.ProcessLine();

            //skip lines containing only comments
            while (Line != null && Line[0] == "c")
            {
                Line = this.ProcessLine();
            }
            if (Line == null || Line.Length != 4)
            {
                Console.WriteLine("Error: incorect format of input file, something missing");
                return false;
            }
            if (Line[0] != "p" || Line[1] != "edge")
            {
                Console.WriteLine("Error: incorect format of input file - p line expected");
                return false;
            }
            if ((!Int32.TryParse(Line[2], out this.NumOfVertices)) || (!Int32.TryParse(Line[3], out this.NumOfEdges)))
            {
                Console.WriteLine("Error: incorect format of input file - p line expected");
                return false;
            }
            int NumVars = this.NumOfColors * this.NumOfVertices;
            int NumClauses = this.NumOfVertices + (this.NumOfVertices * this.NumOfColors * (this.NumOfColors - 1) / 2) + this.NumOfColors * this.NumOfEdges;
            this.Output.AppendLine("c ColoringOfGraph.cnf");
            this.Output.AppendLine("c");
            this.Output.AppendLine($"p cnf {NumVars} {NumClauses}");
            return true;
        }

        /// <summary>
        /// reads line from input file and splits it by spaces
        /// </summary>
        /// <returns></returns>
        private string[] ProcessLine()
        {
            string line = this.Input.ReadLine();
            if(line != null)
            {
                return (line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                return null;
            }
        }
        public void PrintOutput()
        {
            Console.Write(this.Output.ToString());
        }
    }
}
