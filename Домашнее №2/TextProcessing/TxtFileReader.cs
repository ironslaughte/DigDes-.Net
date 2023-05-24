namespace TextProcessing
{
    public class TxtFileReader
    {
        private string _path;

        public TxtFileReader(string path)
        {
            if (!CheckCorrectFormatFile(path)) throw new ArgumentException("Формат файла не .txt");
            _path = path;
        }

        public string ReadFile() => File.ReadAllText(_path);

        private static bool CheckCorrectFormatFile(string path)
        {
            int offset = (".txt").Length;
            int idx = path.LastIndexOf(".txt");
            if(idx != -1)
                return idx + offset == path.Length;

            return false;
        }
    }
}
