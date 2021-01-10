using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinaryBoarding
{
    class Program
    {
        public class BoardingPass
        {

            private int seatID, row, col;

            public BoardingPass()
            {

            }

            public BoardingPass(string raw)
            {
                row = 0;
                for (int i = 0; i < 7; i++)
                {
                    row *= 2;
                    if (raw[i] == 'B')
                    {
                        row += 1;
                    }
                }
                col = 0;
                for (int i = 0; i < 3; i++)
                {
                    col *= 2;
                    if (raw[i+7] == 'R')
                    {
                        col += 1;
                    }
                }
                seatID = row * 8 + col;
            }

            public int SeatID { get => seatID; set => seatID = value; }
            public int Row { get => row; set => row = value; }
            public int Col { get => col; set => col = value; }


        }

        public class BoardingPassComparer : IComparer<BoardingPass>
        {
            public int Compare(BoardingPass x, BoardingPass y)
            {
                if (x.SeatID > y.SeatID)
                    return 1;
                else if (x.SeatID < y.SeatID)
                    return -1;
                return 0;
            }
        }

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
            string[] boardingPassImages = fileContent.Split('\n');
            List<BoardingPass> boardingPasses = new List<BoardingPass>();
            foreach (string image in boardingPassImages)
            {
                if (image.Equals(string.Empty)) continue;
                boardingPasses.Add(new BoardingPass(image));
            }
            boardingPasses.Sort(new BoardingPassComparer());
            int seatID;
            for (int i = 0; i < boardingPasses.Count - 1; i++)
            {
                if (boardingPasses[i + 1].SeatID - boardingPasses[i].SeatID == 2) {
                    seatID = boardingPasses[i].SeatID + 1;
                    break;
                }
            }
        }
    }
}
