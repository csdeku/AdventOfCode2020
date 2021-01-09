using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TobogganTrajectory
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
            string[] slope = fileContent.Split('\n');
            
            Console.WriteLine(
                GetNumberOfTreesInPath(slope, 1, 1)
                * GetNumberOfTreesInPath(slope, 3, 1)
                * GetNumberOfTreesInPath(slope, 5, 1)
                * GetNumberOfTreesInPath(slope, 7, 1)
                * GetNumberOfTreesInPath(slope, 1, 2));
        }

        static long GetNumberOfTreesInPath(string[] slope, int xIncrement, int yIncrement)
        {
            int xPos = 0, patternLength = slope[0].Length;
            long sumOfTreesInPath = 0;
            for (int yPos = 0; yPos < slope.Length; yPos += yIncrement)
            {
                if (xPos >= slope[yPos].Length)
                    continue;
                if (slope[yPos][xPos] == '#')
                    sumOfTreesInPath++;
                xPos = (xPos + xIncrement) % patternLength;
            }
            return sumOfTreesInPath;
        }
    }
}
