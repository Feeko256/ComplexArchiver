using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComplexCompressor.Model
{
    public class FileDetails
    {
        public string FilePath { get; set; }
        public string CompressionSpeed { get; set; }
        public string CompressionType { get; set; }
        public string FileName => Path.GetFileName(FilePath);
        public string FileSize => GetFileSize(FilePath);

        private string GetFileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length.ToString();
        }

    }
}
