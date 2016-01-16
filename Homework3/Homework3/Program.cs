using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace ConsoleApplication1
{
    class Program

    {
        public static void SearchFilename(DateTime modificationsDate, DateTime createDate, DateTime accessDate, int filesize = 0,
                                          string filename = "*",
                                          string path = @"C:\Users\\Natallia_Tsviatkova\\Documents\\GitHub\\c_sharp"
                                           )
        {
            try
            {

                string[] dirs = Directory.GetFiles(path, filename, SearchOption.AllDirectories);
                Console.WriteLine("The number of files starting with c is {0}.", dirs.Length);
                DateTime now = DateTime.Now.Date;

                foreach (string dir in dirs)
                {

                    //Console.WriteLine(dir);
                    FileInfo file = new FileInfo(dir);
                    if ((file.Length >= filesize) && (file.CreationTime.Date >= createDate.Date)
                         && (file.LastAccessTime.Date >= accessDate.Date) && (file.LastWriteTime.Date >= modificationsDate.Date))
                    {
                        Console.WriteLine(dir);
                        Console.WriteLine("{0} byte", file.Length);
                        Console.WriteLine("Creation date: {0}", file.CreationTime.Date);
                        Console.WriteLine("Last access date: {0}", file.LastAccessTime.Date);
                        Console.WriteLine("Last modification date: {0}", file.LastWriteTime.Date);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        public static string DefaultValue(string s, string d)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            else
                return d;
        }

        static void Main(string[] args)
        {
            string buf;
            while (true)
            {
                Console.WriteLine("| 1 - search within file contents useig regexp | 2 - search with filename,creation date,access date | 3 - выход |");
                buf = Console.ReadLine();
                switch (buf)
                {
                    case "1":

                        Console.WriteLine("Enter directory path: ");
                        string path = Console.ReadLine();
                        Console.WriteLine("Enter file name pattern: ");
                        string filename = Console.ReadLine();
                        Console.WriteLine("Enter search pattern: ");
                        string pattern = Console.ReadLine();
                        Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
                        string[] dirs = Directory.GetFiles(path, filename, SearchOption.TopDirectoryOnly);
                        ArrayList ar = new ArrayList();
                        MatchCollection mathCollection;
                        foreach (string files in dirs)
                        {
                            StreamReader str = new StreamReader(files, Encoding.UTF8);
                            string readtext = str.ReadToEnd();
                            str.Dispose();
                            mathCollection = reg.Matches(readtext);
                            if (mathCollection.Count > 0)
                            {
                                ar.Add(files);
                                Console.WriteLine("Do you want to replace all the found string with another? (y/n)");
                                string ans = Console.ReadLine();
                                if (!String.IsNullOrEmpty(ans) && ans.ToLower().StartsWith("y"))
                                {
                                    Console.WriteLine("Enter replacement string: ");
                                    string replacement = Console.ReadLine();
                                    string replaced = reg.Replace(readtext, replacement);
                                    FileStream filestr = new FileStream(files, FileMode.Open, FileAccess.Write, FileShare.Write);
                                    StreamWriter sw = new StreamWriter(filestr, Encoding.UTF8);
                                    sw.Write(replaced);
                                    sw.Dispose();
                                }

                            }
                        }
                        foreach (string f in ar)
                        {
                            Console.WriteLine("Files found: {0}", f);
                        }
                        Console.WriteLine("Do you want to copy files to another directory? (y/n)");
                        string answer = Console.ReadLine();
                        if (!String.IsNullOrEmpty(answer) && answer.ToLower().StartsWith("y"))
                        {
                            Console.WriteLine("Enter new directory: ");
                            string newdir = Console.ReadLine();
                            foreach (string f in ar)
                            {
                                File.Copy(f, Path.Combine(newdir, Path.GetFileName(f)));
                                Console.WriteLine("File {0} copied to directory {1}.", f, newdir);
                            }
                        }
                        Console.WriteLine("Do you want to move files to another directory? (y/n)");
                        answer = Console.ReadLine();
                        if (!String.IsNullOrEmpty(answer) && answer.ToLower().StartsWith("y"))
                        {
                            Console.WriteLine("Enter new directory: ");
                            string newdir = Console.ReadLine();
                            foreach (string f in ar)
                            {
                                File.Move(f, Path.Combine(newdir, Path.GetFileName(f)));
                                Console.WriteLine("File {0} moved.", f);
                            }
                        }
                        Console.WriteLine("Do you want to delete files? (y/n)");
                        answer = Console.ReadLine();
                        if (!String.IsNullOrEmpty(answer) && answer.ToLower().StartsWith("y"))
                        {

                            string newdir = Console.ReadLine();
                            foreach (string f in ar)
                            {
                                File.Delete(f);
                                Console.WriteLine("File {0} deleted.", f);
                            }
                        }

                        break;
                    case "2":
                        //filesize
                        Console.WriteLine("Enter the filesize: ");
                        int fs = int.Parse(Console.ReadLine());
                        //filename
                        Console.WriteLine("Enter the search string: ");
                        string searchStr = Console.ReadLine();
                        //path
                        Console.WriteLine("Enter path: ");
                        path = Console.ReadLine();
                        //creation date
                        Console.WriteLine("Enter creation date in this Format(YYYY-MM-DD): ");
                        DateTime createDate = Convert.ToDateTime(Console.ReadLine());
                        //last access date
                        Console.WriteLine("Enter last access date in this Format(YYYY-MM-DD): ");
                        DateTime accessDate = Convert.ToDateTime(Console.ReadLine());
                        //last modification date
                        Console.WriteLine("Enter last modification date in this Format(YYYY-MM-DD): ");
                        DateTime modificationsDate = Convert.ToDateTime(Console.ReadLine());
                        SearchFilename(modificationsDate, accessDate, createDate, fs, searchStr, path);
                        break;
                    case "3":
                        Console.WriteLine("Отладка - пункт_3 - это выход из цикла while");
                        return; // возвращает из ф-ции значение 3

                    default:
                        char n = (char)Console.Read();
                        Console.WriteLine("Вы нажали клавишу - " + n);
                        break;
                }
            }
        }
    }
}