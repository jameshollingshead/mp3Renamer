using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mp3Renamer
{
    class Program
    {
        static void Main(string[] args)
        {
            string presentWorkingDirectory = Mp3Services.GetPresentWorkingDirectory();
            IEnumerable<string> fileList = Mp3Services.GetMp3FileList(presentWorkingDirectory);

        }
    }
}
