using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using QuickGraph;

namespace HandyHaversacks
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
            string[] bagRules = fileContent.Split('\n');
            /*
             Using a graph we find the vertex for the element we want, i.e. the "shiny golden bag", then count 
            the number of elements connected to it as a parent, i.e. those bags which can contain it. For each of the bags which can contain it, we then count
            the elements connected to them as parents.
            Assuming that there are no circular or two way connections, i.e. that some bag x cannot be both the parent and child of some bag y directly or indirectly.
            This gives us some idea of how to construct the graph from the input and then also how to parse it.
            While the number of bags that can be contained do not come into play initially, there may be further calculations needed in part 2 of the problem.
             */
            

        }
    }
}
