using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
    }
}
