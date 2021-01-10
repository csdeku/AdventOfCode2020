using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomCustoms
{

    class Program
    {
        static void Main(string[] args)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            Thread t = new Thread((ThreadStart)(() =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "D:\\OneDrive\\Shared Folder\\Projects\\AdventOfCode20";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;

                        //Read the contents of the file into a stream
                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileContent = reader.ReadToEnd();
                        }
                    }
                }
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            string[] responseLines = fileContent.Split('\n');
            SortedSet<char> questionSet = new SortedSet<char>();
            int totalUniqueQuestions = 0;
            for (int i = 0; i < responseLines.Length; i++)
            {
                if (responseLines[i].Length == 0)
                {
                    totalUniqueQuestions += questionSet.Count;
                    questionSet.Clear();
                }
                else
                {
                    for (int j = 0; j < responseLines[i].Length; j++)
                    {
                        questionSet.Add(responseLines[i][j]);
                    }
                }
            }

            List<SortedSet<char>> individualQuestionSets = new List<SortedSet<char>>();
            int sumOfTotalUnion = 0;
            for (int i = 0; i < responseLines.Length; i++)
            {
                if (responseLines[i].Length == 0)
                {
                    SortedSet<char> totalUnion = individualQuestionSets[0];
                    for (int j = 1; j < individualQuestionSets.Count; j++)
                    {
                        totalUnion.IntersectWith(individualQuestionSets[j]);
                    }
                    sumOfTotalUnion += totalUnion.Count;
                    individualQuestionSets.Clear();
                }
                else
                {
                    SortedSet<char> setBuilder = new SortedSet<char>();
                    for (int j = 0; j < responseLines[i].Length; j++)
                    {
                        setBuilder.Add(responseLines[i][j]);
                    }
                    individualQuestionSets.Add(setBuilder);
                }
            }

        }
    }
}
