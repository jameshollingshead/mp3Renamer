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

        public static IEnumerable<string> GetMp3FileList(string currentDirectory, bool isRecursive)
        {
            IEnumerable<string> result;

            if(isRecursive)
            {
                result = Directory.EnumerateFiles(currentDirectory, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                result = Directory.EnumerateFiles(currentDirectory, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => s.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }

        public static string DetermineNewFileName(string fileName)
        {
            string result;
            string fileNameOnly;
            TagLib.File fileTag = TagLib.File.Create(fileName);
            string trackNumber = GetTrackNumber(fileTag);
            string songTitle = GetTagMemberWithoutIllegalChars(fileTag.Tag.Title);
            string artistName = GetTagMemberWithoutIllegalChars(fileTag.Tag.FirstPerformer);
                        

            fileNameOnly = artistName + " - " + trackNumber + " - " + songTitle + ".mp3";
            result = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar + fileNameOnly;
            
            return result;
        }

        private static string GetTagMemberWithoutIllegalChars(string fileTagMember)
        {
            string result = fileTagMember;
            //TODO: There has to be a more efficent way to do this using regex
            string[] illegalChars = {"<", ">", @"/", ":", "\"", @"\", "|", "?", "*" };
                        
            foreach(string illegalChar in illegalChars)
            {
                result = result.Replace(illegalChar, String.Empty);
            }

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
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine(fileCount + " files renamed");
        }

    }
}
