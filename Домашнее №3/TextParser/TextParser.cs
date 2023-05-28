namespace TextParser
{
    public static class UniqWordsCounter
    {
        private enum ConnectingLine
        {
            ShortDash = 8211,
            LongDash = 8212,
            Dash = 8208,
            Minus = 8722,
            MinusFromKeyboard = 45
        }

        // массив содержит все возможные виды дефиса в словах
        private static ConnectingLine[] _wordConnectors = new ConnectingLine[]
        {   ConnectingLine.MinusFromKeyboard,
            ConnectingLine.Minus,
            ConnectingLine.Dash,
            ConnectingLine.ShortDash
        };

        // массив содержит основные разделители в тексте, чтобы разбить текст на слова
        private static char[] _separators = new char[]
        { (char)ConnectingLine.LongDash, ' ', '.', ',', '!', '?', ';', ':', '(', ')', '[', ']', '\r', '\t', '\n' };

        private static Dictionary<string, int> PrivateGetAllUniqWordsInText(string text)
        {
            Dictionary<string, int> numberUniqueWords = new Dictionary<string, int>();
            List<string> words = SplitTextIntoWords(text); // разделяем текст на слова

            foreach (string word in words)
            {
                // слово может являться чем-то таким: "«",
                // поэтому дополнительно проверяем на то, что оно состоит из букв и, возможно, дефиса
                string lowerCaseWord = new String(word.Where(ch => Char.IsLetter(ch) || IsDash(ch)).ToArray()).ToLower();
                AddWordToDict(numberUniqueWords, lowerCaseWord);
            }
            return numberUniqueWords;
        }

        public static Dictionary<string, int> PublicGetAllUniqWordsInText(string text)
        {
            Dictionary<string, int> numberUniqueWords = new Dictionary<string, int>();
            // разделяем текст на слова
            List<string> words = text.Split(_separators, System.StringSplitOptions.RemoveEmptyEntries)
                        .AsParallel()
                        .Where(word => !IsDash(word[0])) // гарантировано существует первый непробельный символ
                        .ToList(); // разделяем текст на слова

            object locker = new object();
            List<string> lowerCaseWords = new List<string>();
            Parallel.ForEach(words, word =>
            {
                string lowerCaseWord = new String(word.Where(ch => Char.IsLetter(ch) || IsDash(ch)).ToArray()).ToLower();
                if(lowerCaseWord != null && lowerCaseWord.Length > 0) lowerCaseWords.Add(lowerCaseWord);
                lock (locker)
                {
                    AddWordToDict(numberUniqueWords, lowerCaseWord);
                }
            });
            return numberUniqueWords;
        }

        private static void AddWordToDict(Dictionary<string, int> numberUniqueWords, string lowerCaseWord)
        {
            if (lowerCaseWord != null && lowerCaseWord.Length > 0)
            {
                    if (numberUniqueWords.ContainsKey(lowerCaseWord))
                        numberUniqueWords[lowerCaseWord]++;
                    else
                        numberUniqueWords.Add(lowerCaseWord, 1);
            }
        }

        private static List<string> SplitTextIntoWords(string text)
        {
            // Есть возможность, что вместо длинного тире используется короткое/минус и тп,
            // поэтому нужно исключить из массива слов подобные варианты: "-", "--"
            return text.Split(_separators, System.StringSplitOptions.RemoveEmptyEntries)
                        .Where(word => !IsDash(word[0])) // гарантировано существует первый непробельный символ
                        .ToList();
        }
        private static bool IsDash(char c) => _wordConnectors.Any(s =>
        {
            char line = (char)s;
            return line.Equals(c);
        });
    }

}