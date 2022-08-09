using EtlLibrary.Strategy.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask1
{
    public class Facade
    {
        private static List<string> allFiles = new List<string>();
        private readonly static string extractPath = System.Configuration.ConfigurationManager.AppSettings["ExtractPath"];
        private readonly static string loadPath = System.Configuration.ConfigurationManager.AppSettings["LoadPath"];
        private static int ParsedFiles = 0;
        private static Dictionary<string, int> info = new Dictionary<string, int>();
        private static List<string> InvalidFiles = new List<string>();

        public static void FileProcess()
        {
            DirectoryInfo di = new DirectoryInfo(extractPath);

            allFiles = di.GetFiles().Select(file => file.Name).ToList();

            Reader reader = new Reader();
            int i = 1;
            CreateDateDirectory();
            foreach (var item in allFiles)
            {
                info = reader.Run(extractPath + $"\\{item}", loadPath + "\\" + DateOnly.FromDateTime(DateTime.Now).ToString("MM/dd/yyyy").Replace(".", "-") + $"\\output{i}.json");
                if (info.Count != 0)
                {
                    ParsedFiles++;
                }
                else
                {
                    InvalidFiles.Add(extractPath + $"\\{item}");
                }
                i++;
            }
        }

        private static void CreateDateDirectory()
        {
            try
            {
                string path = loadPath + "\\" + DateOnly.FromDateTime(DateTime.Now).ToString("MM/dd/yyyy");
                path = path.Replace(".", "-");
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        public static bool HasNewFiles()
        {
            DirectoryInfo di = new DirectoryInfo(extractPath);
            var files = di.GetFiles().Select(file => file.Name).ToList();
            bool hasNewFiles = false;
            if (files == allFiles)
            {
                hasNewFiles = true;
            }
            return hasNewFiles;
        }

        public static void CreateLogFile(string date)
        {
            string fileName = loadPath + "\\" + date + "\\meta.log";
            if (!File.Exists(fileName))
            {
                using (StreamWriter writer = File.CreateText(fileName))
                {
                    writer.WriteLine($"parsed_files: {ParsedFiles}");
                    foreach (var entry in info)
                        writer.WriteLine("{0}: {1}", entry.Key, entry.Value);
                    foreach (var item in InvalidFiles)
                    {
                        writer.WriteLine($"invalid_files: {item}");
                    }
                }
            }
        }
    }
}
