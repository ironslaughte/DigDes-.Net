using System.Text;

namespace TextProcessing
{
    public class TxtFileWriter
    {
        private string _fileName;

        public TxtFileWriter(string fileName) => _fileName = fileName;

        public void WriteToFileStatistics(Dictionary<string, int> dict)
        {
            using FileStream fs = File.Create(_fileName);
            if (dict.Count <= 0)
                Write(fs, "Файл был без слов\n");

            var sortedDict = dict.OrderByDescending(x => x.Value).ToList();
            foreach (var word in sortedDict)
            {
                string message = $"{word.Key}  {word.Value}\n";
                Write(fs, message);
            }
        }

        private static void Write(FileStream fs, string message)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(message);
            fs.Write(info, 0, info.Length);
        }
    }
}
