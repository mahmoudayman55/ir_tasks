/*
name : mahmoud ayman mohamed el-ghalban
level : 2
bioinformatics
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task2
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


                List<string> terms = new List<string>();
                List<string> DocumentsID = new List<string>();

                for (int i = 0; i < NumberOfDocs; i++)
                {
                    Console.Write("{0} : ", i + 1);
                    string DocumentContent = Console.ReadLine();
                    NewDocument((i + 1).ToString(), DocumentContent);
                    DocumentsID.Add((i + 1).ToString());
                    ReadDocument((i + 1).ToString(),terms);
                    terms.Remove("\r\n");
                }
                List<string>uniqueTerms=terms.Distinct().ToList();


                List<KeyValuePair<string, string>> Terms_And_IDs = new List<KeyValuePair<string, string>>();
               
                
              
                
            

                int rows = terms.Count() + 1;
              

            
                for (int c = 0; c < NumberOfDocs; c++)
                {
                    StreamReader sr = new StreamReader(DocumentsID[c]);
                    string data = sr.ReadToEnd();
                    sr.Close();
                    List<string> txtFileList = new List<string>(data.Split(' '));
                    txtFileList.Remove("\r\n");
                    foreach (string word in txtFileList)
                    {
                      
                            Terms_And_IDs.Add(new KeyValuePair<string, string>(word, DocumentsID[c]));
                      
                    }
                }
                Console.WriteLine("\n\n\n********************************************************************\n\n\n");

                Console.WriteLine("Term      |id\n----------|---------------------");
                foreach (var term in Terms_And_IDs)
                {
                    Console.Write(term.Key);
                    for (int i = 0; i < 10 - term.Key.Length; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("| " + term.Value+"\n----------|---------------------");
                    
                }
               

                List< int> Terms_And_freq = new List< int>();

                foreach (string file in DocumentsID)
                {
                    counter(file, Terms_And_freq);
                }

              
                Console.WriteLine("\n\n\n********************************************************************\n\n\n");
                
             
                object[,] table = new string[rows, 3];
                table[0, 2] = "freq";
                table[0, 0] = "terms";
                table[0, 1] = "id";

                for (int i = 1; i < terms.Count() + 1; i++)
                {
                    table[i, 0] = terms[i - 1];
                }
               

                for (int i = 1; i < terms.Count() + 1; i++)
                {
                    table[i, 1] = Terms_And_IDs[i - 1].Value;
                }
            

                for (int i = 1; i < terms.Count() + 1; i++)
                {
                    table[i,2] = Terms_And_freq[i - 1].ToString();
                }
              

                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        Console.Write("| " + table[row, col]);

                        for (int i = 0; i < 10 - table[row, col].ToString().Length; i++)
                        {
                            Console.Write(" ");
                        }
                    }

                    Console.WriteLine("\n------------------------------------------------------------------------------------------");
                }

                
                Dictionary<string, List<string>> Terms_And_postingList = new Dictionary<string, List<string>>();
                foreach(string term in uniqueTerms)
                {
                    List<string> PostingList = new List<string>();
                    foreach (string doc in DocumentsID)
                    {
                        StreamReader sr = new StreamReader(doc);
                        string data = sr.ReadToEnd();
                        sr.Close();
                        List<string> txtFileList = new List<string>(data.Split(' '));
                        txtFileList.Remove("\r\n");
                        if (txtFileList.Contains(term))
                        {
                            
                            PostingList.Add(doc);
                        }
                    }
                    Terms_And_postingList.Add(term, PostingList);
                    
                }
                Console.WriteLine("\n\n\n********************************************************************\n\n\n");

                Console.WriteLine("Term      |PostingList      |collectionFreq");
                List<int> freqList = new List<int>();
                collectionfreq(DocumentsID, uniqueTerms, freqList); 

                for (int j=0;j<Terms_And_postingList.Count;j++)
                {
                    Console.Write(Terms_And_postingList.ElementAt(j).Key);
                    for (int i = 0; i < 10 - Terms_And_postingList.ElementAt(j).Key.Length; i++)
                    {
                        Console.Write(" ");
                    }
                    
                    foreach (string lists in Terms_And_postingList.ElementAt(j).Value)
                    {
                        Console.Write(" "+lists);
                    }


                   
                        Console.Write("                  " + freqList[j]);
                    
                    Console.WriteLine("\n");
                }
               
                
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




        static void counter(string file, List<int>terms)
        {
            StreamReader sr = new StreamReader(file);
            string data = sr.ReadToEnd();
      
            List<string> words = new List<string>(data.Split(' '));
            words.Remove("\r\n");
            int currentCount = 0;
             foreach(string word in words)
                {
                currentCount = 0;
                
                    foreach(string word2 in words)
                    {
                        if (word == word2)
                        {
                            currentCount++;
                        }
                    }
                    terms.Add(currentCount);
                }

        }


        static void collectionfreq(List<string>docs, List<string> terms,List<int>freqlist)
        {
            foreach(string term in terms)
            {
                int counter = 0;
                foreach(string doc in docs)
                {
                    StreamReader sr = new StreamReader(doc);
                    string data = sr.ReadToEnd();

                    List<string> words = new List<string>(data.Split(' '));
                    words.Remove("\r\n");
                    foreach(string word in words)
                    {
                        if (word == term)
                        {
                            counter++;
                        }
                    }
                }
                freqlist.Add(counter);
            }


            
           

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
