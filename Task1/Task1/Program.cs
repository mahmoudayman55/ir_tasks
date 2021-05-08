/*
name : mahmoud ayman mohamed el-ghalban
level : 2
bioinformatics
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Warning! if you continue any text file in current folder will be deleted\ncontinue?(y/n)");
            char Continue = char.Parse(Console.ReadLine());
            if (Continue == 'y')
            {
                DeleteTextFiles();



                Console.Write("enter number of documents :  ");
                int NumberOfDocs = int.Parse(Console.ReadLine());


                List<string> words = new List<string>();
                List<string> DocumentsNames = new List<string>();


                for (int i = 0; i < NumberOfDocs; i++)
                {
                    Console.Write("enter the name of document {0} :  ", i + 1);
                    string DocumentNameTxt = Console.ReadLine() + ".txt";
                    string DocumentName = DocumentNameTxt.Replace(".txt", "");
                    DocumentsNames.Add(DocumentName);
                    Console.Write("enter the content of {0} :  ", DocumentName);
                    string DocumentContent = Console.ReadLine();
                    NewDocument(DocumentNameTxt, DocumentContent);
                    ReadDocument(DocumentNameTxt, words);
                }



                List<string> uniqueWords = words.Distinct().ToList();
                uniqueWords.Remove("\r\n");


                int rows = uniqueWords.Count() + 1;
                int columns = NumberOfDocs + 1;

                string[,] table = new string[rows, columns];

                //filling the column 0 with the documents names
                for (int i = 1; i < columns; i++)
                {
                    table[0, i] = DocumentsNames[i - 1];
                }

                //filling the row 0 with the words without redundancy
                for (int i = 1; i < rows; i++)
                {
                    table[i, 0] = uniqueWords[i - 1];

                }


                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
                List<string> filesnames = new List<string>();
                foreach (string file in files)
                {
                    if (file.EndsWith(".txt"))
                    {
                        filesnames.Add(file);
                    }
                }
                for (int r = 1; r < rows; r++)
                {
                    for (int c = 1; c < columns; c++)
                    {
                        StreamReader sr = new StreamReader(filesnames[c - 1]);
                        string data = sr.ReadToEnd();
                        sr.Close();
                        string[] txtFiles = data.Split(' ');
                        List<string> txtFileList = new List<string>(txtFiles);
                        txtFileList.Remove("\r\n");
                        if (txtFileList.Contains(table[r, 0]))
                        {
                            table[r, c] = "1";
                        }
                        else { table[r, c] = "0"; }
                    }
                }


                table[0, 0] = "words";
                Console.WriteLine("\n\n");

                //print the table
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        Console.Write("| " + table[row, col]);

                        for (int i = 0; i < 10 - table[row, col].Length; i++)
                        {
                            Console.Write(" ");
                        }
                    }

                    Console.WriteLine("\n------------------------------------------------------------------------------------------");
                }

                Console.WriteLine("Enter the command line ");
            word1: Console.Write("Word 1 : ");
                string word1 = Console.ReadLine();
                if (!uniqueWords.Contains(word1))
                {
                    Console.WriteLine("Plase enter word from the table");
                    goto word1;
                }
            EnterOperator: Console.Write("Operator (and , or , not) : ");
                string _operator = Console.ReadLine();

                List<string> operators = new List<string> { "and", "or", "not" };
                if (!operators.Contains(_operator))
                {
                    Console.WriteLine("Plase enter valid operator");
                    goto EnterOperator;
                }
            word2: Console.Write("Word 2 : ");

                string word2 = Console.ReadLine();
                if (!uniqueWords.Contains(word2))
                {
                    Console.WriteLine("Plase enter word from the table");
                    goto word2;
                }


                List<string> BinaryWords = new List<string>();
                for (int r = 1; r < rows; r++)
                {
                    if (table[r, 0] == word1 || table[r, 0] == word2)
                    {
                        string BinaryWord = "";
                        for (int c = 1; c < columns; c++)
                        {
                            BinaryWord = BinaryWord + table[r, c];

                        }

                        BinaryWords.Add(BinaryWord);
                    }

                }




                Console.WriteLine("########################################################################### \n \n ");


                int Word1Binary = Convert.ToInt32(BinaryWords.ElementAt(0), 2);
                int Word2Binary = Convert.ToInt32(BinaryWords.ElementAt(1), 2);

                string Result = "";
                switch (_operator)
                {
                    case "and":
                        Result = Convert.ToString((Word1Binary & Word2Binary), 2);
                        break;
                    case "or":
                        Result = Convert.ToString((Word1Binary | Word2Binary), 2);
                        break;
                    case "not":
                        Result = Convert.ToString((Word1Binary & ~Word2Binary), 2);
                        break;


                }
                Console.WriteLine("The result of your command : " + Result);
                Console.WriteLine("Documents matching your search : ");


                for (int i = 1; i < columns; i++)
                {
                    if (Result[i - 1] == '1')
                    {
                        Console.WriteLine(table[0, i]);
                    }
                    else break;
                }


                Console.ReadLine();

            }



            else
            {
                Console.WriteLine("end");
            }
        }















        static void NewDocument(string path, string content)
        {
            TextWriter tw = new StreamWriter(path);
            tw.WriteLine(content + " ");
            tw.Close();
        }







        static void ReadDocument(string path, List<string> list)
        {
            StreamReader sr = new StreamReader(path);
            string data = sr.ReadToEnd();
            string[] words = data.Split(' ');
            foreach (string word in words)
            {
                list.Add(word);

            }

            sr.Close();




        }







        static void DeleteTextFiles()
        {
            string[] filesInDir = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (string file in filesInDir)
            {
                if (file.EndsWith(".txt"))
                    File.Delete(file);

            }
        }





    }
}
