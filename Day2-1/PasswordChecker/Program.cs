using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordChecker
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
            string[] passwordList = fileContent.Split('\n');
            Console.WriteLine(countValid(passwordList));
            Console.WriteLine(countValid2(passwordList));

        }

        static int countValid(string[] passwordTable)
        {
            int totalValid = 0;
            foreach(string line in passwordTable)
            {
                // 3-9 s: bgldhnxsksznscnlnhc
                var regex = Regex.Match(line, @"([0-9]+)-([0-9]+) ([A-Za-z]): (.+)");
                if (regex.Groups.Count < 5) continue;
                int min = int.Parse(regex.Groups[1].Value);
                int max = int.Parse(regex.Groups[2].Value);
                char requiredChar = char.Parse(regex.Groups[3].Value);
                string password = regex.Groups[4].Value;
                int count = password.Count(f => (f == requiredChar));
                if (count >= min && count <= max)
                    totalValid++;
            }

            return totalValid;
        }

        static int countValid2(string[] passwordTable)
        {
            int totalValid = 0;
            foreach (string line in passwordTable)
            {
                // 3-9 s: bgldhnxsksznscnlnhc
                var regex = Regex.Match(line, @"([0-9]+)-([0-9]+) ([A-Za-z]): (.+)");
                if (regex.Groups.Count < 5) continue;
                int posA = int.Parse(regex.Groups[1].Value) - 1;
                int posB = int.Parse(regex.Groups[2].Value) - 1;
                char requiredChar = char.Parse(regex.Groups[3].Value);
                string password = regex.Groups[4].Value;
                bool aIncluded = posA < password.Length && password[posA] == requiredChar;
                bool bIncluded = posB < password.Length && password[posB] == requiredChar;
                if ((aIncluded && !bIncluded) || (!aIncluded && bIncluded))
                {
                    totalValid++;
                }
            }

            return totalValid;
        }






    }
}
