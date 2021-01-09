using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PassportProcessing
{
    public class Passport
    {

        string birthYear, issueYear, expirationYear;
        string passportID, countryID;
        string hairColor, eyeColor, height;

        public Passport()
        {
            birthYear = issueYear = expirationYear = hairColor = eyeColor = height = passportID = countryID = string.Empty;
        }

        public Passport(string input)
        {
            birthYear = issueYear = expirationYear = hairColor = eyeColor = height = passportID = countryID = string.Empty;
            ParseInput(input);
        }

        // A possibly multiline input with a space/line separated list of attributes where the attribute and element are separated by a colon, i.e.:
        // eyr:2022 byr:1972 hcl:#866857 ecl:hzl pid:227453248
        // hgt:153cm cid:324 iyr:2018
        private void ParseInput(string input)
        {
            Regex r = new Regex(@"([^:]+):([\S]+)[\s|\n]+", RegexOptions.Multiline);
            var splitAttributes = r.Matches(input);

            for (int i = 0; i < splitAttributes.Count; i++)
            {
                string attribute = splitAttributes[i].Groups[1].Value;
                string value = splitAttributes[i].Groups[2].Value;
                /*
                byr (Birth Year)
                iyr (Issue Year)
                eyr (Expiration Year)
                hgt (Height)
                hcl (Hair Color)
                ecl (Eye Color)
                pid (Passport ID)
                cid (Country ID)
                */
                switch (attribute)
                {
                    case "byr":
                        birthYear = value;
                        break;
                    case "iyr":
                        issueYear = value;
                        break;
                    case "eyr":
                        expirationYear = value;
                        break;
                    case "hgt":
                        height = value;
                        break;
                    case "hcl":
                        hairColor = value;
                        break;
                    case "ecl":
                        eyeColor = value;
                        break;
                    case "pid":
                        passportID = value;
                        break;
                    case "cid":
                        countryID = value;
                        break;
                    default:
                        break;
                }

                //Console.WriteLine(splitAttributes[i].Groups[1]);
            }
        }

        public bool IsValidPassport()
        {
            return !(birthYear.Equals(string.Empty) || issueYear.Equals(string.Empty) || expirationYear.Equals(string.Empty) || passportID.Equals(string.Empty) || hairColor.Equals(string.Empty)
                || eyeColor.Equals(string.Empty) || height.Equals(string.Empty));
        }

        public bool IsValidPassport2()
        {
            /* 
            byr (Birth Year) - four digits; at least 1920 and at most 2002.
            iyr (Issue Year) - four digits; at least 2010 and at most 2020.
            eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
            hgt (Height) - a number followed by either cm or in:
                If cm, the number must be at least 150 and at most 193.
                If in, the number must be at least 59 and at most 76.
            hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            pid (Passport ID) - a nine-digit number, including leading zeroes.
            cid (Country ID) - ignored, missing or not.
             */
            try
            {
                if (birthYear.Length != 4 || int.Parse(birthYear) < 1920 || int.Parse(birthYear) > 2002)
                    return false;
                if (issueYear.Length != 4 || int.Parse(issueYear) < 2010 || int.Parse(issueYear) > 2020)
                    return false;
                if (expirationYear.Length != 4 || int.Parse(expirationYear) < 2020 || int.Parse(expirationYear) > 2030)
                    return false;
                var reg = Regex.Match(height, @"([0-9]+)(cm|in)");
                int measure = int.Parse(reg.Groups[1].Value);
                string unit = reg.Groups[2].Value;
                if (!unit.Equals("cm") && !unit.Equals("in"))
                    return false;
                else if (unit.Equals("cm") && (measure < 150 || measure > 193))
                    return false;
                else if (unit.Equals("in") && (measure < 59 || measure > 76))
                    return false;
                reg = Regex.Match(hairColor, @"#[0-9a-f]{6}");
                if (!reg.Success)
                    return false;
                reg = Regex.Match(eyeColor, @"amb|blu|brn|gry|grn|hzl|oth");
                if (!reg.Success)
                    return false;
                reg = Regex.Match(passportID, @"[0-9]{9}");
                if (passportID.Length != 9 || !reg.Success)
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }

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
            string[] terminalData = fileContent.Split('\n');
            List<Passport> passports = new List<Passport>();
            string s = string.Empty;
            foreach (string line in terminalData)
            {
                if (line.Equals(string.Empty) && !s.Equals(string.Empty))
                {
                    passports.Add(new Passport(s));
                    s = string.Empty;
                }
                else
                {
                    s = s + line + "\n";
                }
            }
            int sumOfValid = 0;
            int sumOfValid2 = 0;
            foreach (Passport p in passports)
            {
                if (p.IsValidPassport() && p.IsValidPassport2())
                {
                    sumOfValid++;
                    sumOfValid2++;
                }
            }

        }
    }
}
