using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagLib;

namespace Mp3Renamer
{
    public static class Mp3Services
    {
        public static string GetPresentWorkingDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public static IEnumerable<string> GetMp3FileList(string currentDirectory)
        {
            var result = Directory.EnumerateFiles(currentDirectory, "*.*", SearchOption.TopDirectoryOnly)
            .Where(s => s.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase));

            return result;
        }

        public static string DetermineNewFileName(string fileName)
        {
            string result;
            string fileNameOnly;
            TagLib.File fileTag = TagLib.File.Create(fileName);
            string trackNumber = GetTrackNumber(fileTag);
                        

            fileNameOnly = fileTag.Tag.FirstPerformer +" - " + trackNumber + " - " + fileTag.Tag.Title + ".mp3";
            result = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar + fileNameOnly;
            
            return result;
        }

        private static string GetTrackNumber(TagLib.File fileTag)
        {
            string result;

            //We want track numbers to be in the form of NN, so track 1 is "01" and track 10 is "10"
            if (fileTag.Tag.Track < 10)
                result = "0" + fileTag.Tag.Track.ToString();
            else
                result = fileTag.Tag.Track.ToString();

            return result;
        }

        public static void RenameFiles(IEnumerable<string> fileList)
        {
            string newFileName;
            int fileCount = 0;

            foreach(string file in fileList)
            {
                newFileName = DetermineNewFileName(file);
                try
                {
                    System.IO.File.Move(file, newFileName);
                    fileCount++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Unable to rename file - " + Path.GetFileName(file));
                }
            }

            Console.WriteLine(fileCount + " files renamed");
        }

    }
}
